namespace RuleEngine;

public delegate void RuleCheck(IEntity entity);

public class Rule
{
    public string Name { get; }
    public string TargetEntityType { get; }
    public RuleCheck Check { get; }

    public Rule(string name, string targetEntityType, RuleCheck check)
    {
        Name = name;
        TargetEntityType = targetEntityType;
        Check = check;
    }

    public bool AppliesTo(IEntity entity) => entity.EntityType == TargetEntityType;
}