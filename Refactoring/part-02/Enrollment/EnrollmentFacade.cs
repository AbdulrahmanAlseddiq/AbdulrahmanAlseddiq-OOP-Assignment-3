namespace RefactoringLab.Part02.Enrollment;

/// <summary>
/// Facade: one simple entry point for enrolling a student.
/// It hides the four subsystem services and the order in which they must be called.
/// The services in Services.cs are used as they are — none of them was changed.
/// </summary>
public sealed class EnrollmentFacade
{
    private readonly PaymentGateway _payment;
    private readonly SeatInventory _seats;
    private readonly InvoiceGenerator _invoices;
    private readonly EmailService _email;

    public EnrollmentFacade()
        : this(new PaymentGateway(), new SeatInventory(), new InvoiceGenerator(), new EmailService())
    {
    }

    // Lets a caller (or a test) supply the subsystem objects if it wants to.
    public EnrollmentFacade(PaymentGateway payment, SeatInventory seats, InvoiceGenerator invoices, EmailService email)
    {
        _payment = payment ?? throw new ArgumentNullException(nameof(payment));
        _seats = seats ?? throw new ArgumentNullException(nameof(seats));
        _invoices = invoices ?? throw new ArgumentNullException(nameof(invoices));
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    /// <summary>Enrolls the student and returns the invoice id.</summary>
    public string Enroll(string studentId, string courseId, decimal amount)
    {
        _payment.Charge(studentId, amount);
        _seats.Reserve(courseId, studentId);
        var invoiceId = _invoices.Create(studentId, amount);
        _email.Send(studentId, "Enrollment confirmed", $"Invoice {invoiceId} for {courseId}");
        return invoiceId;
    }
}
