# Refactoring, Patterns & Generics — OOP Assignment 3

| | |
|---|---|
| **Student** | Abdulrahman Alseddiq |
| **GitHub** | [@AbdulrahmanAlseddiq](https://github.com/AbdulrahmanAlseddiq) |
| **Cohort** | net-g1 |
| **Branch / PR** | `assignment/1-7` → `main`, PR title `OOP Assignment 3 [S1-A7]` |
| **Stack** | C# 14 / .NET 10, console apps, no external NuGet packages |

## Contents

| Part | Folder | What's inside |
|---|---|---|
| 01 — Refactoring lab | [`Refactoring/`](Refactoring/) | `part-01` (OCP strategies, DIP, composition — [`Refactoring.md`](Refactoring/part-01/Refactoring.md)), `part-02` (Template Method, Facade — [`Answers.md`](Refactoring/part-02/Answers.md)), `part-03` (HashSet, `yield` — [`Answers.md`](Refactoring/part-03/Answers.md)) |
| 02 — Collections & extension methods | [`Collections/`](Collections/) | [`Answers.md`](Collections/Answers.md) (Tasks 2.1, 2.2) + `src/` with `StringValidationExtensions.cs` and the test-table `Program.cs` |
| 03 — Generics | [`Generics/`](Generics/) | `Store<T> where T : IHasId`, `EnumerableExtensions`, Step 7 `Program.cs`; [`Answers.md`](Generics/Answers.md) (Steps 2, 3, 7 + last question) |

## How to run

```bash
dotnet run --project Refactoring/part-01   # original output + the new carrier (Bosta) and channel (WhatsApp)
dotnet run --project Refactoring/part-02   # writes the 3 reports, enrolls through the facade
dotnet run --project Refactoring/part-03   # CountBlocked timing + first 3 students
dotnet run --project Collections/src       # PASS/FAIL for every row of the table (exit code 0 = all pass)
dotnet run --project Generics/src          # Step 7
```

## Commit history

The history shows how the design changed:

- **Refactoring** — the original lab is imported unchanged first, then one commit per refactoring
  (shipping → orders → notifications → new carrier → new channel → reports → facade → …).
  The R5/R6 commits (`feat(part-01): add Bosta carrier …`, `… WhatsApp notification channel …`) only add one
  new file and edit `Program.cs` — proof that no existing class was edited.
  The `part-03` "before" timing was committed **before** the code was changed.
- **Generics** — one commit per step, named `Step 1:` … `Step 7:`. Every commit builds; in the Step 3 commit
  the line that does not compile (`item.Id`) is kept commented out with the real compiler error next to it.

## Notes & assumptions

- **Part 01** — The three projects were copied into `Refactoring/` as they are (same project names,
  `net10.0`). `Program.cs` is the composition root: it creates the carriers, the repository/e-mail sender/clock
  and the channels and passes them in. `OrderProcessor` now reads the clock once (the original read
  `DateTime.Now` twice, so the saved time and the e-mail time could differ). Notification **options** (urgent,
  scheduled) are properties of one `Notification` class composed with any `INotificationChannel`.
- **Part 03 timings** were measured with `dotnet run` (Debug) in the same environment before and after:
  `CountBlocked` 25–26 ms → 8–9 ms, `found=5000` both times. Absolute numbers differ per machine.
- **Collections / Generics** — no LINQ inside the methods I wrote (only `foreach`, `yield`, `List`,
  `Dictionary`, `HashSet`, `Regex`). LINQ remains only inside the original lab's report formatters
  (Part 01 code, which the no-LINQ rule does not cover).
- **Phone regex** — `^(?:0|\+20)1[0125][0-9]{8}\z`. `[0-9]` instead of `\d` (which also matches Arabic-Indic
  digits) and `\z` instead of `$` (which would accept a trailing new line). Spaces or dashes inside the number
  are treated as invalid — the input is not trimmed or cleaned.
- **National ID regex** — `^[23][0-9]{13}\z`: format only (14 digits, first is 2 or 3); the birth date and
  governorate code inside the ID are not validated, as the task asks only for the format.
- **`Store<T>`** — a duplicate Id throws `InvalidOperationException`; `GetAll()` returns
  `IReadOnlyCollection<T>` (a read-only view of the dictionary's values, no copy).
- No `bin/` or `obj/` folders are committed (`.gitignore`).
