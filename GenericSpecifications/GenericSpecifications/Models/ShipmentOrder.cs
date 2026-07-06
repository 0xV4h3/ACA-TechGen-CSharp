namespace GenericSpecifications.Models;

public class ShipmentOrder(string id, decimal weightKg, bool isFragile, bool isExpress, string zone)
{
    public string Id { get; }  = id;
    public decimal WeightKg { get; } = weightKg;
    public bool IsFragile { get; } = isFragile;
    public bool IsExpress { get; } = isExpress;
    public string Zone { get; } = zone;
}