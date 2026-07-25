using Domain.Configuration;
using Domain.Constants;
using Domain.Models;
using Domain.Converters;
using Domain.Quality;
using Domain.Registries;
using Domain.Simulation;

namespace FactoryCore;

public sealed class FactoryPipelineSimulation
{
    private readonly AppConfig _config;
    private readonly TickEngine _engine = new();
    private readonly SimulationStats _stats = new();
    private readonly TickLog _tickLog = new(60);
    private readonly Random _random;

    private readonly OrderLine _orderLine;
    private readonly Storage _storage;
    private readonly StockLocation _stock;
    private readonly List<StockOrder> _orders;
    private readonly List<MachineProductionSchedule> _machineSchedules;
    private readonly QualityCheckerProcessor _qualityProcessor;
    private readonly TransportSchedule _transportSchedule;

    private int _nextItemId;
    private int _executedTicks;

    public FactoryPipelineSimulation(IRegistry registry)
    {
        _config = Configuration.Settings;
        _random = new Random(_config.Simulation.RandomSeed);
        _nextItemId = _config.Simulation.StartItemId;

        var supportedItemTypes = new List<ItemType> { ItemTypes.A, ItemTypes.B, ItemTypes.C };

        _orderLine = new OrderLine(
            OrderLineTypes.Standard,
            registry,
            _config.Simulation.OrderLineCapacity,
            CapacityConverters.ForOrderLine.Default);

        _storage = new Storage(
            StorageTypes.Standard,
            registry,
            supportedItemTypes,
            _config.Storage.StorageCapacityPerType,
            CapacityConverters.ForStorage.Default);

        _stock = new StockLocation(
            StockTypes.Standard,
            "Main Stock",
            registry,
            supportedItemTypes,
            _config.Storage.StockCapacityPerType,
            CapacityConverters.ForStock.Default);
        
        _orders =
        [
            new StockOrder(_stock, ItemTypes.A, int.MaxValue),
            new StockOrder(_stock, ItemTypes.B, int.MaxValue),
            new StockOrder(_stock, ItemTypes.C, int.MaxValue)
        ];

        var checker = new ItemQualityChecker(
            ItemQualityChecker.BuildThresholds(_config.QualityChecker.PassPercentage),
            registry,
            onPassed: item =>
            {
                if (!_storage.TryStore(item))
                    _tickLog.Add($"Storage FULL -> passed item #{item.Id} of type '{item.Type}' was lost " +
                                 "(known limitation: Storage has no retry buffer for this build).");
            },
            onRepair: item => _tickLog.Add($"Item #{item.Id} sent to repair (not re-queued in this build)."),
            onScrap: item => _tickLog.Add($"Item #{item.Id} scrapped."));

        _qualityProcessor = new QualityCheckerProcessor(
            checker,
            _orderLine,
            _config.QualityChecker.MinTicksPerItem,
            _config.QualityChecker.MaxTicksPerItem,
            _random,
            _stats,
            _tickLog.Add);

        var fleet = new List<Domain.Abstractions.ITransportVehicle> { new Truck(), new Ship(), new CargoPlane(), new Motorcycle() };
        var hub = new TransportHub(fleet, new CheapestSufficientVehicleStrategy());

        _transportSchedule = new TransportSchedule(
            hub,
            _storage,
            _orders,
            _config.Transport.ArrivalIntervalTicks,
            _config.Transport.CapacityPerArrival,
            _stats,
            _tickLog.Add);

        var premiumQualityStrategy = new NormalDistributionQuality(mean: 85.0, stdDev: 5.0);
        var standardQualityStrategy = new NormalDistributionQuality(mean: 70.0, stdDev: 10.0);
        var poorQualityStrategy = new NormalDistributionQuality(mean: 50.0, stdDev: 15.0);
        
        var factoryA = new FactoryA(registry, poorQualityStrategy);
        var factoryB = new FactoryB(registry, standardQualityStrategy);
        var factoryC = new FactoryC(registry, premiumQualityStrategy);

        _machineSchedules =
        [
            new MachineProductionSchedule(
                (SingleTypeMachine)factoryA.CreateMachine(), _orderLine,
                _config.Production.MachineA.IntervalTicks, _config.Production.MachineA.TotalItemsToProduce,
                NextItemId, _stats, _tickLog.Add),
            new MachineProductionSchedule(
                (SingleTypeMachine)factoryB.CreateMachine(), _orderLine,
                _config.Production.MachineB.IntervalTicks, _config.Production.MachineB.TotalItemsToProduce,
                NextItemId, _stats, _tickLog.Add),
            new MachineProductionSchedule(
                (SingleTypeMachine)factoryC.CreateMachine(), _orderLine,
                _config.Production.MachineC.IntervalTicks, _config.Production.MachineC.TotalItemsToProduce,
                NextItemId, _stats, _tickLog.Add)
        ];

        foreach (var schedule in _machineSchedules) _engine.Register(schedule);
        _engine.Register(_qualityProcessor);
        _engine.Register(_transportSchedule);
    }

    private int NextItemId() => _nextItemId++;

    public void Run()
    {
        Console.WriteLine();
        Console.WriteLine("Starting simulation...");
        Console.WriteLine();

        int tick = 0;
        int totalTicks = _config.Simulation.TotalSimulationTicks;

        if (totalTicks == -1)
        {
            Console.WriteLine("Auto-complete mode enabled: run until all planned items are in final state.");
            Console.WriteLine();

            while (true)
            {
                tick++;
                RunOneTick(tick, $"Tick {tick} (auto)");
                if (IsFullyDrained()) break;
            }
        }
        else
        {
            for (tick = 1; tick <= totalTicks; tick++)
            {
                RunOneTick(tick, $"Tick {tick}/{totalTicks}");
            }

            while (!IsFullyDrained())
            {
                tick++;
                RunOneTick(tick, $"Tick {tick} (drain)");
            }
        }

        PrintFinalSummary();
    }

    private void RunOneTick(int tick, string header)
    {
        _tickLog.Reset();
        _executedTicks = tick;

        _engine.RunTick(tick);

        Console.WriteLine("--------------------------------------------------------------");
        Console.WriteLine(header);
        Console.WriteLine("--------------------------------------------------------------");
        _tickLog.Print();
        Console.WriteLine();
        Console.WriteLine($"ORDER LINE: {_orderLine.Count}/{_orderLine.CapacityLimit} ({_orderLine.Fill.Constant})");
        Console.WriteLine($"QUALITY CHECKER: {(_qualityProcessor.IsIdle ? "Idle" : "Processing")}");
        Console.WriteLine($"STORAGE   : A={_storage.CountFor(ItemTypes.A)}, B={_storage.CountFor(ItemTypes.B)}, C={_storage.CountFor(ItemTypes.C)}");
        Console.WriteLine($"STOCK     : A={_stock.CountFor(ItemTypes.A)}, B={_stock.CountFor(ItemTypes.B)}, C={_stock.CountFor(ItemTypes.C)} [{_stock.State}]");
        Console.WriteLine();
    }

    private bool IsFullyDrained() =>
        _orderLine.Count == 0
        && _storage.TotalCount == 0
        && _qualityProcessor.IsIdle
        && _machineSchedules.All(m => m.IsProductionComplete);

    private void PrintFinalSummary()
    {
        Console.WriteLine("==============================================================");
        Console.WriteLine("Final Summary");
        Console.WriteLine("==============================================================");
        Console.WriteLine(_stats.SummaryFor(ItemTypes.A));
        Console.WriteLine(_stats.SummaryFor(ItemTypes.B));
        Console.WriteLine(_stats.SummaryFor(ItemTypes.C));
        Console.WriteLine("--------------------------------------------------------------");
        Console.WriteLine($"Total in Order Line : {_orderLine.Count}");
        Console.WriteLine($"Total in Storage    : {_storage.TotalCount}");
        Console.WriteLine($"Total ticks run     : {_executedTicks}");
        Console.WriteLine("==============================================================");
    }
}
