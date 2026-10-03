namespace RefactoringLab;

public sealed class DhlCarrier : IShippingCarrier
{
    public string Name => "DHL";

    public decimal Calculate(decimal weightKg) => weightKg * 18m;
}
