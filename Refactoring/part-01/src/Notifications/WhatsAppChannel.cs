namespace RefactoringLab;

/// <summary>
/// New channel added for R6 — only this file was created and it was wired in Program.cs.
/// It gets urgent / scheduled / both for free from <see cref="Notification"/>.
/// </summary>
public sealed class WhatsAppChannel : INotificationChannel
{
    public string Name => "whatsapp";

    public void Deliver(string header, string to, string message) =>
        Console.WriteLine($"[{header}] {to}: {message}");
}
