# Part 01 — answers

---

## ShippingCostCalculator

- **What was the problem?**
  Every carrier and its price were hard-coded inside one `switch`. Adding a carrier (or changing a price)
  meant opening and editing `ShippingCostCalculator` again — it was **not closed for modification (OCP)**.
  The class also had more than one reason to change: one per carrier. Carrier names were magic strings
  that were only checked at run time.

- **What did you change?**
  - Added the `IShippingCarrier` interface (`Name`, `Calculate(weightKg)`).
  - Each carrier is now its own class that owns its own price: `AramexCarrier`, `FedExCarrier`, `DhlCarrier`
    (Strategy pattern).
  - `ShippingCostCalculator` receives the carriers through its constructor, stores them in a
    `Dictionary<string, IShippingCarrier>` and only *looks up* the carrier and delegates to it.
    It keeps the same public call `Calculate("Aramex", 2)` and still throws `ArgumentException` for an
    unknown carrier. A carrier registered twice is rejected at construction time.

---

## OrderProcessor

- **What was the problem?**
  `Process` created its own dependencies with `new SqlOrderRepository()` and `new SmtpEmailSender()`, and read
  `DateTime.Now` directly. The high-level business rule was **hard-wired to low-level details (DIP violation)**:
  you could not swap SQL for another store, send through another e-mail provider, or test it without really
  saving and sending. It also called `DateTime.Now` **twice**, so the saved time and the time in the e-mail
  could be different moments.

- **What did you change?**
  - Added `IOrderRepository` and `IEmailSender`; `SqlOrderRepository` and `SmtpEmailSender` implement them.
  - `OrderProcessor` now gets `IOrderRepository`, `IEmailSender` and a `TimeProvider` (the built-in .NET clock
    abstraction) through its **constructor** (constructor injection) and never uses `new` for a dependency.
  - The clock is read once, so the saved time and the e-mail time are the same.
  - The objects are composed in `Program.cs` (the composition root).

---

## Notifications

- **What was the problem?**
  Two independent things — *the channel* (email / SMS) and *the options* (urgent / scheduled) — were combined
  through **inheritance**, so every combination needed its own class
  (`UrgentEmailNotification`, `UrgentScheduledEmailNotification`, `UrgentScheduledSmsNotification`, ...).
  This is a class explosion: channels × options. "Scheduled but not urgent" did not even exist, the
  scheduled classes re-wrote the `[URGENT]` text instead of reusing it, and adding one new channel would
  require a whole new family of subclasses.

- **What did you change?** (composition over inheritance)
  - `INotificationChannel` — *how* a message is delivered (`EmailChannel`, `SmsChannel`). A channel knows
    nothing about urgency or scheduling.
  - `Notification` — a single sealed class that **has a** channel and has the options as plain properties
    (`IsUrgent`, `SendAt`). It builds the header (`email` / `email scheduled …`) and the body (`[URGENT] …`)
    once, for every channel.
  - Any channel can now be sent **urgent, scheduled, or both** without a class per combination; the old
    seven-class tree was removed. The two calls in `Program.cs` print exactly the same lines as before.

---

## Proof

- **New carrier file(s):** `src/Shipping/BostaCarrier.cs` (wired in `Program.cs` — `Bosta 2kg → 20`)
  — commit `feat(part-01): add Bosta carrier as a new class only (R5)`.
- **New notification channel file(s):** `src/Notifications/WhatsAppChannel.cs` (wired in `Program.cs`, sent
  urgent, scheduled and both) — commit `feat(part-01): add WhatsApp notification channel as a new class only (R6)`.
- **Existing classes left unchanged? (yes/no):** **Yes.** Each of those two commits only adds the new file and
  edits `Program.cs` (`git show --stat` lists nothing else).

### Output — before vs after (R4)

Before refactoring:

```text
Aramex 2kg → 24
FedEx 2kg  → 30

[SQL] save order 1001 @ 2026-10-03T12:38:30.0709643+03:00
[SMTP] to=customer@example.com body=Order 1001 confirmed at 10/03/2026 12:38:30

[email scheduled 10/03/2026 18:00] customer@example.com: [URGENT] Your order ships tomorrow
[sms] +201000000000: [URGENT] OTP 4821
```

After refactoring (same lines; the new carrier and the new channel are extra lines):

```text
Aramex 2kg → 24
FedEx 2kg  → 30
Bosta 2kg  → 20

[SQL] save order 1001 @ 2026-10-03T12:39:50.1263711+03:00
[SMTP] to=customer@example.com body=Order 1001 confirmed at 10/03/2026 12:39:50

[email scheduled 10/03/2026 18:00] customer@example.com: [URGENT] Your order ships tomorrow
[sms] +201000000000: [URGENT] OTP 4821

[email] student: [URGENT] urgent only
[email scheduled 10/03/2026 20:00] student: scheduled only
[email scheduled 10/03/2026 20:00] student: [URGENT] urgent + scheduled
[sms] student: [URGENT] urgent only
[sms scheduled 10/03/2026 20:00] student: scheduled only
[sms scheduled 10/03/2026 20:00] student: [URGENT] urgent + scheduled
[whatsapp] student: [URGENT] urgent only
[whatsapp scheduled 10/03/2026 20:00] student: scheduled only
[whatsapp scheduled 10/03/2026 20:00] student: [URGENT] urgent + scheduled
```

(Times are the moment the program ran; the date format follows the machine's culture.)
