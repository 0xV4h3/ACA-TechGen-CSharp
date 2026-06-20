namespace Domain.Configuration;

public class StorageSettings
{
    private int _storageCapacityPerType;
    private int _stockCapacityPerType;

    public int StorageCapacityPerType
    {
        get => _storageCapacityPerType;
        set
        {
            if (value <= 0)
                throw new ArgumentException("StorageCapacityPerType must be greater than 0.");
            _storageCapacityPerType = value;
        }
    }

    public int StockCapacityPerType
    {
        get => _stockCapacityPerType;
        set
        {
            if (value <= 0)
                throw new ArgumentException("StockCapacityPerType must be greater than 0.");
            _stockCapacityPerType = value;
        }
    }
}