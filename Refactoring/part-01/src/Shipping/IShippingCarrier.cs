namespace RefactoringLab;

/// <summary>
/// One shipping carrier and its own pricing rule.
/// A new carrier is a new class that implements this interface — no existing code changes.
/// </summary>
public interface IShippingCarrier
{
    string Name { get; }

    decimal Calculate(decimal weightKg);
}
