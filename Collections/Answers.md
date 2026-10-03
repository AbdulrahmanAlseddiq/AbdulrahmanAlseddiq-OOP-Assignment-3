# Collections — Answers

## Task 2.1 — Research: `IReadOnlyDictionary<TKey, TValue>` & `SortedDictionary<TKey, TValue>`

### What is each one?

- **`IReadOnlyDictionary<TKey, TValue>`** is an **interface**, not a collection by itself. It describes a
  key/value collection that you can only **read**: get a value by key (`this[key]`, `TryGetValue`), check
  `ContainsKey`, read `Keys`, `Values`, `Count`, and loop over it. It has **no** `Add`, `Remove`, `Clear` or
  setter. `Dictionary`, `SortedDictionary`, `ConcurrentDictionary`, `FrozenDictionary` and
  `ReadOnlyDictionary` all implement it.
- **`SortedDictionary<TKey, TValue>`** is a **real collection** of key/value pairs that is **always kept sorted
  by key**. Inside it is a balanced binary search tree (a red-black tree), not a hash table. It sorts with
  `Comparer<TKey>.Default` (the key's `IComparable<T>`) or with an `IComparer<TKey>` you pass in.

### How is `IReadOnlyDictionary` different from `Dictionary`?

| | `Dictionary<TKey, TValue>` | `IReadOnlyDictionary<TKey, TValue>` |
|---|---|---|
| Kind | concrete class (a hash table) | interface (a "view"/contract) |
| Read (`[key]`, `TryGetValue`, `ContainsKey`, `Keys`, loop) | ✅ | ✅ |
| Write (`Add`, `Remove`, `Clear`, `[key] = value`) | ✅ | ❌ — not part of the interface |
| Lookup speed | O(1) average | whatever the object behind it does |

`Dictionary` *implements* `IReadOnlyDictionary`, so a `Dictionary` can be returned **as** an
`IReadOnlyDictionary` with no copy.

### Why would a public method return `IReadOnlyDictionary` instead of `Dictionary`?

- **Encapsulation** — the class keeps its own private `Dictionary` and stays the only one that can change
  it. If it returned the `Dictionary` itself, any caller could `Add`, `Remove` or `Clear` the object's
  internal data and break its rules.
- **Clear intent** — the return type tells the caller "you may read this, not change it", and the compiler
  enforces it: `prices.Add(...)` simply does not compile.
- **Freedom to change later** — the method promises only "something I can look up by key". The class can
  later switch to a `SortedDictionary`, a `FrozenDictionary`, etc. without breaking callers.
- Note: the interface stops changes *through that reference*. A caller could still cast it back to
  `Dictionary`; for a hard guarantee, wrap it in `ReadOnlyDictionary<TKey,TValue>`
  (`dictionary.AsReadOnly()`), which throws on any write.

### How is `SortedDictionary` different from `Dictionary`?

| | `Dictionary` | `SortedDictionary` |
|---|---|---|
| Inside | hash table | balanced binary search tree (red-black) |
| **Order** when you loop | **not guaranteed** (do not rely on it) | **always sorted by key** |
| Lookup / insert / remove | **O(1)** average | **O(log n)** |
| Key needs | `GetHashCode` + `Equals` | must be comparable (`IComparable<T>` or an `IComparer<T>`) |
| Memory | lower | higher (a tree node per item) |

So `Dictionary` is faster for plain lookups; `SortedDictionary` pays O(log n) for every operation in exchange
for always being in key order.

### When would I choose `SortedDictionary`?

When I need key/value lookups **and** the data must always be read in key order, while items keep being
added and removed in random order — e.g. a timetable keyed by start time, a leaderboard keyed by score, an
order book keyed by price. Sorting a `Dictionary` every time I print it would cost O(n log n) each time; the
`SortedDictionary` keeps the order as it goes. (If the data is loaded once, already sorted, and rarely
changes, `SortedList` uses less memory.) If I never need the order, I choose `Dictionary`.

### Sources

1. Microsoft Learn — [IReadOnlyDictionary&lt;TKey,TValue&gt; Interface](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2)
2. Microsoft Learn — [SortedDictionary&lt;TKey,TValue&gt; Class](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2) (binary search tree, O(log n) retrieval, comparison with `SortedList`)
3. Microsoft Learn — [ReadOnlyDictionary&lt;TKey,TValue&gt; Class](https://learn.microsoft.com/dotnet/api/system.collections.objectmodel.readonlydictionary-2)
4. Redgate Simple Talk — [SortedDictionary and SortedList](https://www.red-gate.com/simple-talk/blogs/sorteddictionary-and-sortedlist/)
5. InfoWorld — [How to use SortedDictionary, SortedList, and SortedSet in C#](https://www.infoworld.com/article/3615672/how-to-use-sorteddictionary-sortedlist-and-sortedset-in-csharp.html)

---

## Task 2.2 — Pick the Collection

| # | Scenario | Collection | Why (one sentence) |
|---|---|---|---|
| S1 | Find a student by national ID — thousands of times a day | **`Dictionary<string, Student>`** | Keyed by national ID, each lookup is O(1) on average instead of scanning the whole list every time. |
| S2 | Course tags — the same tag must never be stored twice | **`HashSet<string>`** | A set ignores duplicates by design (`Add` returns `false`) and checks "already there?" in O(1). |
| S3 | A student's grades in the order entered — duplicates allowed | **`List<int>`** | A list keeps insertion order and allows the same value more than once, which a set or dictionary cannot. |
| S4 | A public method returns the course price list — read-only for callers | **`IReadOnlyDictionary<string, decimal>`** | Callers can look up and read prices, but the interface has no `Add`/`Remove`/setter, so they cannot change the class's data. |
| S5 | Timetable keyed by session start time — added any time, always printed in time order | **`SortedDictionary<DateTime, Session>`** | It keeps itself sorted by key on every insert (O(log n)), so it always prints in time order without re-sorting. |
| S6 | Results the caller loops over once and may stop early | **`IEnumerable<T>`** (with `yield return`) | Items are produced lazily one at a time, so if the caller stops early the rest are never created and no full list is built. |
