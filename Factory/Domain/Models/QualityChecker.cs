using Domain.Utils;
using Domain.Quality;

namespace Domain.Models;

public abstract class QualityChecker : Entity<QualityCheckerType, QualityCheckerState>
{
    private readonly IRangeConverter<QualityRoute> _router;
    private readonly Action<Item> _onPassed;
    private readonly Action<Item> _onRepair;
    private readonly Action<Item> _onScrap;

    protected QualityChecker(
        QualityCheckerType type,
        QualityCheckerState state,
        List<RangeStep<QualityRoute>> thresholds,
        IRegistry registry,
        Action<Item> onPassed,
        Action<Item> onRepair,
        Action<Item> onScrap)
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

    public QualityRoute Check(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        EnsureCheckerIsReady();

        var route = _router.Convert(item.Quality.Percentage);

        switch (route)
        {
            case QualityRoute.Passed: _onPassed(item); break;
            case QualityRoute.Repair: _onRepair(item); break;
            case QualityRoute.Scrap:  _onScrap(item); break;
        }

        return route;
    }
}