namespace RefactoringLab;

public sealed class AramexCarrier : IShippingCarrier
{
    public string Name => "Aramex";

    public decimal Calculate(decimal weightKg) => weightKg * 12m;
}
