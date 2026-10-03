namespace RefactoringLab;

public class Notification
{
    public virtual void Send(string to, string message) =>
        Console.WriteLine($"notify {to}: {message}");
}

public class EmailNotification : Notification
{
    public override void Send(string to, string message) =>
        Console.WriteLine($"[email] {to}: {message}");
}

public class SmsNotification : Notification
{
    public override void Send(string to, string message) =>
        Console.WriteLine($"[sms] {to}: {message}");
}

public class UrgentEmailNotification : EmailNotification
{
    public override void Send(string to, string message) =>
        base.Send(to, $"[URGENT] {message}");
}

public class UrgentSmsNotification : SmsNotification
{
    public override void Send(string to, string message) =>
        base.Send(to, $"[URGENT] {message}");
}

public class UrgentScheduledEmailNotification : UrgentEmailNotification
{
    public DateTime SendAt { get; set; }

    public override void Send(string to, string message) =>
        Console.WriteLine($"[email scheduled {SendAt:g}] {to}: [URGENT] {message}");
}

public class UrgentScheduledSmsNotification : UrgentSmsNotification
{
    public DateTime SendAt { get; set; }

    public override void Send(string to, string message) =>
        Console.WriteLine($"[sms scheduled {SendAt:g}] {to}: [URGENT] {message}");
}
