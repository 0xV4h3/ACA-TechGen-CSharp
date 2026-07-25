namespace Domain.Configuration;

public class StorageSettings
{
    public int StorageCapacityPerType { get; init; }
    public int StockCapacityPerType { get; init; }

    public void Validate()
    {
        if (StorageCapacityPerType <= 0) throw new ArgumentException("StorageCapacityPerType must be greater than 0.");
        if (StockCapacityPerType <= 0) throw new ArgumentException("StockCapacityPerType must be greater than 0.");
    }
}