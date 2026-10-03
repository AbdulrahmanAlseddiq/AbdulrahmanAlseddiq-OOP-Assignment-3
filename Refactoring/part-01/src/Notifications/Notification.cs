namespace RefactoringLab;

/// <summary>
/// A notification is COMPOSED of a channel (how it is sent) plus independent options
/// (urgent? scheduled?). Any channel can be combined with any options, so there is no
/// class per combination (UrgentScheduledEmail..., UrgentSms..., ...) any more.
/// </summary>
public sealed class Notification
{
    private readonly INotificationChannel _channel;

    public Notification(INotificationChannel channel)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    public bool IsUrgent { get; init; }

    /// <summary>null = send now.</summary>
    public DateTime? SendAt { get; init; }

    public void Send(string to, string message)
    {
        var header = SendAt is { } sendAt
            ? $"{_channel.Name} scheduled {sendAt:g}"
            : _channel.Name;

        var body = IsUrgent ? $"[URGENT] {message}" : message;

        _channel.Deliver(header, to, body);
    }
}
