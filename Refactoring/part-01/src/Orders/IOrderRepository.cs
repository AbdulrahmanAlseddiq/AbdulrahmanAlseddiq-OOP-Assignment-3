namespace RefactoringLab;

public interface IOrderRepository
{
    void Save(int orderId, DateTime processedAt);
}
