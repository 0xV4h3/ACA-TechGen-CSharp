using Domain.Constants;
using Domain.Registries;
using Domain.Exceptions;
using Domain.Models.Quality;

namespace Domain.Models;

public abstract class QualityChecker : Entity
{
    private readonly List<QualityThreshold> _thresholds;
    
    private readonly Action<IQualitative> _onPassed;
    private readonly Action<IQualitative> _onRepair;
    private readonly Action<IQualitative> _onScrap;

    protected QualityChecker(
        QualityCheckerType type, 
        QualityCheckerState state, 
        List<QualityThreshold> thresholds,
        IRegistry registry,
        Action<IQualitative> onPassed,
        Action<IQualitative> onRepair,
        Action<IQualitative> onScrap)
        : base(type, state)
    {
        if (registry == null) throw new ArgumentNullException(nameof(registry));
        
        if (!registry.IsValid(type.Value, Contexts.Types.QualityChecker))
            throw new TypeException($"Invalid quality checker type '{type.Value}' for context '{Contexts.Types.QualityChecker.Name}'", type);

        if (thresholds == null || !thresholds.Any())
            throw new ArgumentException("Quality checker must have at least one quality threshold rule.");
        
        _thresholds = thresholds.OrderByDescending(t => t.MinPercentage).ToList();
        _onPassed = onPassed ?? throw new ArgumentNullException(nameof(onPassed));
        _onRepair = onRepair ?? throw new ArgumentNullException(nameof(onRepair));
        _onScrap = onScrap ?? throw new ArgumentNullException(nameof(onScrap));
    }
    
    protected virtual bool IsCheckerReady() => State != QualityCheckerStates.Maintenance;
    
    protected virtual void EnsureCheckerIsReady()
    {
        if (!IsCheckerReady())
            throw new InvalidOperationException("The quality checker is undergoing maintenance and cannot check.");
    }
    
    public QualityRoute Check(IQualitative qualitative)
    {
        if (qualitative == null) throw new ArgumentNullException(nameof(qualitative));
        
        EnsureCheckerIsReady();
        
        var matchedRule = _thresholds.FirstOrDefault(t => qualitative.QualityPercentage >= t.MinPercentage);
        
        var route = matchedRule?.Route ?? QualityRoute.Scrap;
        
        switch (route)
        {
            case QualityRoute.Passed:
                _onPassed(qualitative);
                break;
            case QualityRoute.Repair:
                _onRepair(qualitative);
                break;
            case QualityRoute.Scrap:
                _onScrap(qualitative);
                break;
        }

        return route;
    }
}