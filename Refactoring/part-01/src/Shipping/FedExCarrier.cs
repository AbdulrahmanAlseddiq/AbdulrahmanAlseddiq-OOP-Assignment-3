namespace RefactoringLab;

public sealed class FedExCarrier : IShippingCarrier
{
    public string Name => "FedEx";

    public decimal Calculate(decimal weightKg) => weightKg * 15m;
}
