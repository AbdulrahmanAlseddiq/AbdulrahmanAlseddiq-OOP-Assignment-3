namespace RefactoringLab;

/// <summary>
/// High-level order workflow. It depends only on abstractions (IOrderRepository, IEmailSender,
/// TimeProvider) that are handed in through the constructor — it never creates a concrete
/// SQL repository, SMTP sender, or reads the system clock by itself (DIP).
/// </summary>
public sealed class OrderProcessor
{
    private readonly IOrderRepository _repository;
    private readonly IEmailSender _emailSender;
    private readonly TimeProvider _clock;

    public OrderProcessor(IOrderRepository repository, IEmailSender emailSender, TimeProvider clock)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public void Process(int orderId, string customerEmail)
    {
        // Read the clock once so the saved time and the time in the e-mail are the same moment.
        var processedAt = _clock.GetLocalNow().LocalDateTime;

        _repository.Save(orderId, processedAt);
        _emailSender.Send(customerEmail, $"Order {orderId} confirmed at {processedAt}");
    }
}
