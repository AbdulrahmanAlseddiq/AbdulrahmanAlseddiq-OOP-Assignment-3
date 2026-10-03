# Part 02 — answers

---

## Reports

- **What was the problem?**
  `CsvReportExporter`, `JsonReportExporter` and `TextReportExporter` were three copies of the same class.
  `Export`, `Load`, `Validate` and `Save` were identical, character for character — only `Format` was
  different. The export *algorithm* (load → validate → format → save) was written three times, so a fix or
  a new step (e.g. logging, a different data source) had to be made in three places and could easily get
  out of sync.

- **What did you change?**
  - Added the **abstract class** `ReportExporter` with the **template method** `Export(path)`.
    It is *not* virtual, so the steps and their order are written in exactly one place and a subclass cannot
    change or skip them.
  - The shared steps live in the base class: `Load` and `Validate` (`protected virtual` — same for all, but a
    future report may override them) and `Save` (`private` — never changes).
  - `Format` is `protected abstract` — the one step every exporter must provide.
  - Each exporter is now a few lines and keeps only what is different about it: its `Format` override.
  - The three report files are still written with byte-for-byte the same content (checked with `cmp`
    against the files produced before the refactoring).

- **Why did you choose that approach? — why an abstract class and not an interface?**
  - The exporters share **code and state of the algorithm**, not only a contract. An interface can only say
    *"you must have Export"*; every class would still have to write `Load`, `Validate`, `Save` and the order of
    the steps itself — the exact duplication we wanted to remove.
  - The Template Method needs a **non-overridable method that calls overridable steps**. An abstract class gives
    that: a concrete `Export` that fixes the order, `protected abstract` steps the child *must* fill in,
    `protected virtual` steps with a default, and `private` steps the child cannot see. Interface members are
    public by nature, so the inner steps (`Format`, `Validate`) would leak out to every caller.
  - (C# default interface methods could hold some shared code, but they cannot hold fields, cannot have
    protected-only steps that the class implements, and an implementing class can re-implement `Export` and
    break the order — so they do not give the "one place, fixed order" guarantee.)
  - The three exporters really **are** kinds of the same thing ("a report exporter"), so an *is-a*
    relationship is natural here.

---

## Enrollment

- **What was the problem?**
  To enroll one student, `Program.cs` had to create four services itself (`PaymentGateway`,
  `SeatInventory`, `InvoiceGenerator`, `EmailService`), know which ones are needed, call them **in the right
  order**, and pass the invoice id from one call to the next. Every caller that enrolls a student would have
  to repeat this workflow and could get the order wrong (e.g. reserve a seat before charging). The caller was
  tightly coupled to the whole subsystem.

- **What did you change?**
  - Added `EnrollmentFacade` with one method: `Enroll(studentId, courseId, amount)` that returns the invoice id.
  - The facade owns the four services and calls them in the correct order: charge → reserve seat → create
    invoice → send confirmation e-mail.
  - `Program.cs` now enrolls with one call and no longer creates or calls any of the four services.
  - `Services.cs` was not touched.
  - The facade also has a constructor that receives the four services, so they can be supplied from outside
    (e.g. in tests); the parameterless constructor keeps the call site simple.

- **Why did you choose that approach? — what problem does the Facade solve, and what does the caller no longer need to know?**
  The Facade gives a **simple front door to a complex subsystem**. The caller now only needs to know
  *"enroll this student in this course for this amount"*. It no longer needs to know:
  - which services exist and how to create them;
  - the order of the steps;
  - that the invoice id from `InvoiceGenerator` must be passed into the e-mail;
  - what text the confirmation e-mail uses.

  If the workflow changes (a new step, a different order), only the facade changes — not every caller.
  The subsystem classes are still available for advanced use; the facade does not hide them by force,
  it just makes the common case simple.

### Output — same before and after

```text
=== Reports ===
Wrote reports to /tmp/refactoring-lab-part02

=== Enrollment ===
charge S100: 1500
reserve seat CS201 for S100
invoice INV-S100-1500
email S100: Enrollment confirmed / Invoice INV-S100-1500 for CS201
```
