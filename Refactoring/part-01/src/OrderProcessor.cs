namespace RefactoringLab;

public class OrderProcessor
{
    public void Process(int orderId, string customerEmail)
    {
        var repo = new SqlOrderRepository();
        var email = new SmtpEmailSender();

        repo.Save(orderId, DateTime.Now);
        email.Send(customerEmail, $"Order {orderId} confirmed at {DateTime.Now}");
    }
}

public class SqlOrderRepository
{
    public void Save(int orderId, DateTime processedAt) =>
        Console.WriteLine($"[SQL] save order {orderId} @ {processedAt:O}");
}

public class SmtpEmailSender
{
    public void Send(string to, string body) =>
        Console.WriteLine($"[SMTP] to={to} body={body}");
}
