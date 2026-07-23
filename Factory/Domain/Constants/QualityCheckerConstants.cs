namespace Domain.Constants;

public class QualityCheckerType : TypeConstant
{
    internal QualityCheckerType(string value) : base(value, Contexts.QualityChecker) { }
}

public static class QualityCheckerTypes
{
    public static QualityCheckerType Create(string value) => new(value);
    
    public static readonly QualityCheckerType Standard = new("Standard");
    public static readonly QualityCheckerType QualityCheckerA = new("A");
    public static readonly QualityCheckerType QualityCheckerB = new("B");
    public static readonly QualityCheckerType QualityCheckerC = new("C");
    public static readonly QualityCheckerType Unknown = new("Unknown");
}

public class QualityCheckerState : StateConstant
{
    internal QualityCheckerState(string value) : base(value, Contexts.QualityChecker) { }
}

public static class QualityCheckerStates
{
    public static QualityCheckerState Create(string value) => new(value);
    
    public static readonly QualityCheckerState Idle = new("Idle");
    public static readonly QualityCheckerState Checking = new("Checking");
    public static readonly QualityCheckerState Passed = new("Passed");
    public static readonly QualityCheckerState Failed = new("Failed");
    public static readonly QualityCheckerState Maintenance = new("Maintenance");
}