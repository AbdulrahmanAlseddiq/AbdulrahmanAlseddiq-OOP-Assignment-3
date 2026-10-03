namespace RefactoringLab;

/// <summary>
/// Looks up the carrier by name and lets the carrier price the parcel.
/// The calculator no longer knows any carrier or any price — it is closed for modification
/// and open for extension (new carriers are passed in from outside).
/// </summary>
public sealed class ShippingCostCalculator
{
    private readonly Dictionary<string, IShippingCarrier> _carriers;

    public ShippingCostCalculator(IEnumerable<IShippingCarrier> carriers)
    {
        ArgumentNullException.ThrowIfNull(carriers);

        _carriers = new Dictionary<string, IShippingCarrier>();
        foreach (var carrier in carriers)
        {
            if (!_carriers.TryAdd(carrier.Name, carrier))
                throw new ArgumentException($"Carrier '{carrier.Name}' is registered more than once.", nameof(carriers));
        }
    }

    public decimal Calculate(string carrier, decimal weightKg)
    {
        if (!_carriers.TryGetValue(carrier, out var found))
            throw new ArgumentException($"Unknown carrier: {carrier}");

        return found.Calculate(weightKg);
    }
}
