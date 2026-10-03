# Part 03 — answers

---

## BlockedUsers

- **Time complexity before:** **O(n × m)** — for each of the *m* request ids, `List<int>.Contains` does a
  linear scan over the *n* blocked ids (n = 50,000, m = 5,000). In this run every request is found, so each
  scan stops on average half-way: ≈ 5,000 × 25,000 = 125,000,000 comparisons.
- **Time (ms) before:** **25–26 ms** (`dotnet run`, Debug, 5 runs: 25, 25, 26, 26, 26) — `found=5000`.
- **What did you change?**
  `CountBlocked` now copies the blocked ids into a `HashSet<int>` **once** and checks every request with
  `HashSet.Contains`. A hash set finds an item by computing its hash and jumping to its bucket, instead of
  walking through the list item by item. The method signature, `Program.cs` and the result are unchanged —
  `found` is still **5000**.
- **Time complexity after:** **O(n + m)** — O(n) to build the set once, then O(1) on average for each of the
  *m* lookups.
- **Time (ms) after:** **8–9 ms** (same machine and settings, 5 runs: 8, 9, 8, 8, 8) — `found=5000`.
  Almost all of that time is building the set of 50,000 items (and JIT warm-up); the 5,000 lookups themselves
  are well under 1 ms. The gap grows with the data: in theory, doubling both lists makes the old version ≈ 4× slower but
  the new one only ≈ 2× slower.

| | Complexity | Time | found |
|---|---|---|---|
| Before (`List.Contains`) | O(n × m) | 25–26 ms | 5000 |
| After (`HashSet.Contains`) | O(n + m) | 8–9 ms | 5000 |

> Measured inside the course container (.NET 10, Debug build). Absolute numbers on another PC will differ,
> but the before/after ratio is what matters.

---

## Students

- **What was the problem?**
  `GetAllStudents()` returned a `List<Student>`, so it was **eager**: before the caller received even the
  first student, it had already created **1,000,000** `Student` objects and 1,000,000 strings and stored them
  all in a list. `Program.cs` then printed 3 and `break` — the other 999,997 students were created for
  nothing, costing time and a lot of memory (the whole program took ≈ 500 ms, mostly for this).

- **What did you change?**
  `GetAllStudents()` now returns `IEnumerable<Student>` and uses `yield return` inside the loop, so it is
  **lazy (deferred execution)**: the compiler turns the method into an iterator, and each student is created
  only when the `foreach` asks for the next item. When `Program.cs` breaks after 3, the iterator stops — only
  **3** students are ever created and no list is built. The output is the same (`1: Student 1` … `printed=3`)
  and the whole program now takes ≈ 50 ms instead of ≈ 500 ms.
