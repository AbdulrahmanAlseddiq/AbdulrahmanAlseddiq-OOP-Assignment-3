using RefactoringLab;

var shipping = new ShippingCostCalculator(
[
    new AramexCarrier(),
    new FedExCarrier(),
    new DhlCarrier(),
    new BostaCarrier(),   // new carrier (R5)
]);
Console.WriteLine($"Aramex 2kg → {shipping.Calculate("Aramex", 2)}");
Console.WriteLine($"FedEx 2kg  → {shipping.Calculate("FedEx", 2)}");
Console.WriteLine($"Bosta 2kg  → {shipping.Calculate("Bosta", 2)}");
Console.WriteLine();

var processor = new OrderProcessor(
    new SqlOrderRepository(),
    new SmtpEmailSender(),
    TimeProvider.System);
processor.Process(1001, "customer@example.com");
Console.WriteLine();

INotificationChannel email = new EmailChannel();
INotificationChannel sms = new SmsChannel();

new Notification(email) { IsUrgent = true, SendAt = DateTime.Today.AddHours(18) }
    .Send("customer@example.com", "Your order ships tomorrow");
new Notification(sms) { IsUrgent = true }
    .Send("+201000000000", "OTP 4821");
Console.WriteLine();

// Every channel × every option — including the new channel (R6) — with no class per combination.
INotificationChannel whatsapp = new WhatsAppChannel();
var tonight = DateTime.Today.AddHours(20);
foreach (var channel in new[] { email, sms, whatsapp })
{
    new Notification(channel) { IsUrgent = true }
        .Send("student", "urgent only");
    new Notification(channel) { SendAt = tonight }
        .Send("student", "scheduled only");
    new Notification(channel) { IsUrgent = true, SendAt = tonight }
        .Send("student", "urgent + scheduled");
}
