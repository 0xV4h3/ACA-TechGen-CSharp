namespace RuleEngine;

public class RuleEngine
{
    private Rule[] _rules;
    private int _count;

    public RuleEngine(int initialCapacity = 4)
    {
        if (initialCapacity <= 0)
            initialCapacity = 4;

        _rules = new Rule[initialCapacity];
        _count = 0;
    }

    public void AddRule(Rule rule)
    {
        if (_count == _rules.Length)
            Array.Resize(ref _rules, _rules.Length * 2);

        _rules[_count++] = rule;
    }

    public void ValidateFailFast(IEntity entity)
    {
        for (int i = 0; i < _count; i++)
        {
            var rule = _rules[i];
            if (!rule.AppliesTo(entity))
                continue;

            try
            {
                rule.Check(entity);
            }
            catch (RuleViolationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RuleViolationException(rule.Name, $"Unexpected rule error:{ex.Message}");
            }
        }
    }

    public void ValidateCollectAll(IEntity entity)
    {
        var violations = new List<RuleViolationException>();

        for (int i = 0; i < _count; i++)
        {
            var rule = _rules[i];
            if (!rule.AppliesTo(entity))
                continue;

            try
            {
                rule.Check(entity);
            }
            catch (RuleViolationException ex)
            {
                violations.Add(ex);
            }
            catch (Exception ex)
            {
                violations.Add(new RuleViolationException(rule.Name, $"Unexpected rule error:{ex.Message}"));
            }
        }

        if (violations.Count > 0)
            throw new EntityValidationException(entity, violations.ToArray());
    }
}