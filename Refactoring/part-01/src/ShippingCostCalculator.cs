namespace RefactoringLab;

public class ShippingCostCalculator
{
    public decimal Calculate(string carrier, decimal weightKg)
    {
        switch (carrier)
        {
            case "Aramex":
                return weightKg * 12m;
            case "FedEx":
                return weightKg * 15m;
            case "DHL":
                return weightKg * 18m;
            default:
                throw new ArgumentException($"Unknown carrier: {carrier}");
        }
    }
}
