# Part 03 — answers

---

## BlockedUsers

- **Time complexity before:** **O(n × m)** — for each of the *m* request ids, `List<int>.Contains` does a
  linear scan over the *n* blocked ids (n = 50,000, m = 5,000 → up to 250,000,000 comparisons).
- **Time (ms) before:** **25–26 ms** (`dotnet run`, Debug, 5 runs: 25, 25, 26, 26, 26) — `found=5000`.
- What did you change?
- Time complexity after:
- Time (ms) after:

---

## Students

- What was the problem?
- What did you change?
