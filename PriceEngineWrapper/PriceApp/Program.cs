using ACA.PriceEngine;
using AcaWrapperPriceEngine;

namespace PriceApp;

class Program
{
    static void Main(string[] args)
    {
        PriceEngine engine = new();
        PriceEngineWrapper engineWrapper = new();
        PriceInput priceInput = new PriceInput
        {
            Lines = [
                new BasketLine() { Sku = "COFFEE", Quantity = 8, UnitPrice = 10},
                new BasketLine() { Sku = "TEA", Quantity = 4, UnitPrice = 10}
            ],
            LoyaltyTier = 2,
            CouponAmount = 20m,
            VatRate = 0.2m
        };

        decimal wrongAmount = engine.CalculatePayable(priceInput);
        Console.WriteLine($"Wrong amount: {wrongAmount}");
        
        decimal amount = engineWrapper.CalculatePayable(priceInput);
        Console.WriteLine($"Correct amount: {amount}");
    }
}