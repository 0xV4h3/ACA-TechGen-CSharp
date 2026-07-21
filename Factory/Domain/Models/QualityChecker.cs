using Domain.Constants;
using Domain.Registries;
using Domain.Utils;
using Domain.Models.Quality;
using Domain.Models.Converters;
using Domain.Models.Abstractions;

namespace Domain.Models;

public abstract class QualityChecker : Entity
{
    private readonly IRangeConverter<QualityRoute> _router;
    private readonly Action<GradedConstant<ItemQuality>> _onPassed;
    private readonly Action<GradedConstant<ItemQuality>> _onRepair;
    private readonly Action<GradedConstant<ItemQuality>> _onScrap;

    protected QualityChecker(
        QualityCheckerType type, QualityCheckerState state,
        List<RangeStep<QualityRoute>> thresholds, IRegistry registry,
        Action<GradedConstant<ItemQuality>> onPassed,
        Action<GradedConstant<ItemQuality>> onRepair,
        Action<GradedConstant<ItemQuality>> onScrap)
        : base(type, state)
    {
        registry.Validate(type, state);
        _router = new SteppedRangeConverter<QualityRoute>(thresholds);
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

    public QualityRoute Check(GradedConstant<ItemQuality> quality)
    {
        if (quality == null) throw new ArgumentNullException(nameof(quality));
        EnsureCheckerIsReady();

        var route = _router.Convert(quality.Percentage);
        switch (route)
        {
            case QualityRoute.Passed: _onPassed(quality); break;
            case QualityRoute.Repair: _onRepair(quality); break;
            case QualityRoute.Scrap:  _onScrap(quality); break;
        }
        return route;
    }
}