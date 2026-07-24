using Domain.Constants;

namespace Domain.Models;

public class StockOrder
{
    public StockLocation Destination { get; }
    public ItemType ItemType { get; }
    public int Quantity { get; }
    public int Delivered { get; private set; }

    public bool IsFulfilled => Delivered >= Quantity;
    public int Remaining => Quantity - Delivered;

    public StockOrder(StockLocation destination, ItemType itemType, int quantity)
    {
        Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        ItemType = itemType ?? throw new ArgumentNullException(nameof(itemType));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0.", nameof(quantity));

        Quantity = quantity;
    }

    public void RegisterDelivery(int count)
    {
        if (count <= 0) return;
        Delivered = Math.Min(Quantity, Delivered + count);
    }
}