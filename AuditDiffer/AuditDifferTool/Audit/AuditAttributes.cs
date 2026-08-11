namespace AuditDifferTool.Audit;

[AttributeUsage(AttributeTargets.Property)]
public sealed class AuditIgnoreAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public sealed class AuditNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}