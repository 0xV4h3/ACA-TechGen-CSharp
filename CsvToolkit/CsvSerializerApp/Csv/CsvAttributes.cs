namespace CsvSerializerApp.Csv;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CsvIgnoreAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public sealed class CsvColumnAttribute(string header) : Attribute
{
    public string Header { get; } = header;
    public int Order { get; set; } = int.MaxValue;
}