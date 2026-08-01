using System.Reflection;
using ACA.PriceEngine;

namespace AcaWrapperPriceEngine;

public class PriceEngineWrapper
{
    private readonly PriceEngine _priceEngine;
    
    private readonly Func<List<BasketLine>, decimal> _computeSubtotal;
    private readonly Func<decimal, int, decimal> _applyVolumeDiscount;
    private readonly Func<decimal, int, decimal> _applyLoyaltyDiscount;
    private readonly Func<decimal, decimal, decimal> _applyCoupon;
    private readonly Func<decimal, decimal, decimal> _applyVat;
    private readonly Func<List<BasketLine>, int> _countUnits;
    
    private readonly Func<decimal, decimal> _roundMoney;

    public PriceEngineWrapper()
    {
        _priceEngine = new PriceEngine();
        var engineType = typeof(PriceEngine);
        
        _computeSubtotal = CreateInstanceDelegate<Func<List<BasketLine>, decimal>>(engineType, "ComputeSubtotal");
        _countUnits = CreateInstanceDelegate<Func<List<BasketLine>, int>>(engineType, "CountUnits");
        _applyVolumeDiscount = CreateInstanceDelegate<Func<decimal, int, decimal>>(engineType, "ApplyVolumeDiscount");
        _applyLoyaltyDiscount = CreateInstanceDelegate<Func<decimal, int, decimal>>(engineType, "ApplyLoyaltyDiscount");
        _applyCoupon = CreateInstanceDelegate<Func<decimal, decimal, decimal>>(engineType, "ApplyCoupon");
        _applyVat = CreateInstanceDelegate<Func<decimal, decimal, decimal>>(engineType, "ApplyVat");
        
        _roundMoney = CreateStaticDelegate<Func<decimal, decimal>>(engineType, "RoundMoney");
    }

    public decimal CalculatePayable(PriceInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        
        var amount = _computeSubtotal(input.Lines);
        var units = _countUnits(input.Lines);

        amount = _applyVolumeDiscount(amount, units);
        amount = _applyLoyaltyDiscount(amount, input.LoyaltyTier);
        amount = _applyCoupon(amount, input.CouponAmount);
        amount = _applyVat(amount, input.VatRate);

        return _roundMoney(amount);
    }
    
    private TDelegate CreateInstanceDelegate<TDelegate>(Type type, string methodName) where TDelegate : Delegate
    {
        var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new MissingMethodException(type.Name, methodName);
        
        return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), _priceEngine, method);
    }
    
    private TDelegate CreateStaticDelegate<TDelegate>(Type type, string methodName) where TDelegate : Delegate
    {
        var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new MissingMethodException(type.Name, methodName);

        return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), method);
    }
}
