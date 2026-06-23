namespace RuleEngine;

public class RuleViolationException(string ruleName, string message) : Exception(message)
{
    public string RuleName { get; } = ruleName;
}

public class EntityValidationException(
    IEntity entity, RuleViolationException[] violations) : Exception($"{entity.EntityType} #{entity.Id} has {violations.Length} validation error(s).")
{
    public IEntity Entity { get; } = entity;
    public RuleViolationException[] Violations { get; } = violations;
}