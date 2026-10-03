namespace RefactoringLab;

public sealed class EmailChannel : INotificationChannel
{
    public string Name => "email";

    public void Deliver(string header, string to, string message) =>
        Console.WriteLine($"[{header}] {to}: {message}");
}
