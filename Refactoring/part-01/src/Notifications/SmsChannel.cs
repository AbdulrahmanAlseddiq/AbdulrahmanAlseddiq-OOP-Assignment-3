namespace RefactoringLab;

public sealed class SmsChannel : INotificationChannel
{
    public string Name => "sms";

    public void Deliver(string header, string to, string message) =>
        Console.WriteLine($"[{header}] {to}: {message}");
}
