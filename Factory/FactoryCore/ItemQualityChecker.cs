using Domain.Constants;
using Domain.Models;
using Domain.Quality;
using Domain.Converters;
using Domain.Registries;

namespace FactoryCore;

public sealed class ItemQualityChecker(
    List<RangeStep<QualityRoute>> thresholds,
    IRegistry registry,
    Action<Item> onPassed,
    Action<Item> onRepair,
    Action<Item> onScrap)
    : QualityChecker(QualityCheckerTypes.Standard, QualityCheckerStates.Idle, thresholds, registry, onPassed, onRepair, onScrap)
{
    public static List<RangeStep<QualityRoute>> BuildThresholds(int passPercentage)
    {
        if (passPercentage is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(passPercentage));

        int repairFloor = passPercentage / 2;

        return
        [
            new RangeStep<QualityRoute>(passPercentage, QualityRoute.Passed),
            new RangeStep<QualityRoute>(repairFloor, QualityRoute.Repair),
            new RangeStep<QualityRoute>(0, QualityRoute.Scrap)
        ];
    }
}
