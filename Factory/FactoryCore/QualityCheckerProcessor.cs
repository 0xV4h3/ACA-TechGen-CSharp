using Domain.Models;
using Domain.Models.Quality;
using Domain.Simulation;

namespace FactoryCore;

public sealed class QualityCheckerProcessor : ITickable
{
    private readonly ItemQualityChecker _checker;
    private readonly OrderLine _orderLine;
    private readonly int _minTicksPerItem;
    private readonly int _maxTicksPerItem;
    private readonly Random _random;
    private readonly SimulationStats _stats;
    private readonly Action<string> _log;

    private Item? _current;
    private int _ticksRemaining;

    public bool IsIdle => _current is null;

    public QualityCheckerProcessor(
        ItemQualityChecker checker,
        OrderLine orderLine,
        int minTicksPerItem,
        int maxTicksPerItem,
        Random random,
        SimulationStats stats,
        Action<string> log)
    {
        _checker = checker ?? throw new ArgumentNullException(nameof(checker));
        _orderLine = orderLine ?? throw new ArgumentNullException(nameof(orderLine));
        _minTicksPerItem = minTicksPerItem;
        _maxTicksPerItem = maxTicksPerItem;
        _random = random ?? throw new ArgumentNullException(nameof(random));
        _stats = stats ?? throw new ArgumentNullException(nameof(stats));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public void Tick(int tickNumber)
    {
        if (_current is null)
        {
            if (!_orderLine.TryDequeue(out var item) || item is null) return;

            _current = item;
            _ticksRemaining = _random.Next(_minTicksPerItem, _maxTicksPerItem + 1);
            _log($"Quality checker started processing item #{_current.Id} ({_ticksRemaining} ticks).");
            return;
        }

        _ticksRemaining--;
        if (_ticksRemaining > 0) return;
        
        var route = _checker.Check(_current);
        var bucket = route switch
        {
            QualityRoute.Passed => _stats.Passed,
            QualityRoute.Repair => _stats.Repaired,
            _ => _stats.Scrapped
        };
        _stats.Increment(bucket, _current.Type);
        _log($"Item #{_current.Id} checked -> {route} (quality {_current.Quality.Percentage}%).");

        _current = null;
    }
}
