namespace RefactoringLab;

/// <summary>
/// HOW a message travels (email, SMS, ...). A channel knows nothing about urgency or scheduling.
/// A new channel is a new class that implements this interface — no existing code changes.
/// </summary>
public interface INotificationChannel
{
    /// <summary>Short name shown in the delivery header, e.g. "email".</summary>
    string Name { get; }

    /// <param name="header">Header built by <see cref="Notification"/>, e.g. "email" or "email scheduled 10/3/2026 6:00 PM".</param>
    void Deliver(string header, string to, string message);
}
