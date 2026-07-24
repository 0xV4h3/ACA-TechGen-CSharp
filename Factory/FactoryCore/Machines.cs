using Domain.Constants;
using Domain.Models;
using Domain.Models.Quality;

namespace FactoryCore;

public class MachineA : SingleTypeMachine
{
    public MachineA()
        : base(MachineTypes.MachineA, MachineStates.Idle, ItemTypes.A) { }
    
    public MachineA(IQualityGenerator qualityGenerator) 
        : base(MachineTypes.MachineA, MachineStates.Idle, ItemTypes.A, qualityGenerator) { }

    protected override Item CreateItem(int id, int qualityPercentage) 
        => new ItemA(id, qualityPercentage);
}

public class MachineB : SingleTypeMachine
{
    public MachineB()
        : base(MachineTypes.MachineB, MachineStates.Idle, ItemTypes.B) { }
    
    public MachineB(IQualityGenerator qualityGenerator) 
        : base(MachineTypes.MachineB, MachineStates.Idle, ItemTypes.B, qualityGenerator) { }

    protected override Item CreateItem(int id, int qualityPercentage) 
        => new ItemB(id, qualityPercentage);
}

public class MachineC : SingleTypeMachine
{
    public MachineC()
        : base(MachineTypes.MachineC, MachineStates.Idle, ItemTypes.C) { }
    
    public MachineC(IQualityGenerator qualityGenerator) 
        : base(MachineTypes.MachineC, MachineStates.Idle, ItemTypes.C, qualityGenerator) { }

    protected override Item CreateItem(int id, int qualityPercentage) 
        => new ItemC(id, qualityPercentage);
}