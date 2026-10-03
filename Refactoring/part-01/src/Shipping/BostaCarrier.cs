namespace RefactoringLab;

/// <summary>
/// New carrier added for R5 — only this file was created and it was wired in Program.cs.
/// No existing class was edited.
/// </summary>
public sealed class BostaCarrier : IShippingCarrier
{
    public string Name => "Bosta";

    public decimal Calculate(decimal weightKg) => weightKg * 10m;
}
