# Generics — Answers

## Step 2 — `StudentStore` vs `CourseStore`: what is the same, what is different?

**The same:**
- The **whole logic** is identical, line for line: a private `List<...>`, the same four methods
  (`Add`, `GetById`, `GetAll`, `Remove`), the same `foreach` that compares `.Id == id`, the same
  "return `null` if not found", the same `Remove` that first finds the item and then removes it.
- Both entity classes have an `int Id`, and that `Id` is the only member the store actually uses.

**The different:**
- **Only the type name**: `Student` ↔ `Course` (in the field, the parameters and the return types) and the
  variable names. The store never touches `Name`, `Title` or `Price`.

So the two classes are copy-paste duplicates that differ only in the type. Every bug fix or new feature
(e.g. "reject a duplicate id") would have to be written twice — and once more for every new entity
(teachers, rooms, ...). The type should be a **parameter** → a generic class.

## Step 3 — One class for both: why `GetById` does not compile

Compiler error (copied from `dotnet build`):

```text
Store.cs(17,22): error CS1061: 'T' does not contain a definition for 'Id' and no accessible extension method
'Id' accepting a first argument of type 'T' could be found (are you missing a using directive or an assembly
reference?)
```

**Why the compiler rejects it — in my own words:**

C# generics are checked **once, when `Store<T>` itself is compiled**, not later for each type I plug in.
At that moment the compiler does not know what `T` will be: it could be `Student`, but it could just as
well be `int`, `string` or `DateTime` — none of which has an `Id`. Without any constraint, the only thing
the compiler can promise about `T` is that it is *some* `object`, so it only allows the members of `object`
(`ToString`, `Equals`, `GetHashCode`, `GetType`). `item.Id` is not one of them, so it is a compile-time error.

It does not matter that both `Student` and `Course` happen to have a property called `Id` — they are two
unrelated properties on two unrelated classes; nothing in the code says "every `T` has an `Id`". To fix it
I have to **tell the compiler** that `T` always has an `Id` — with an interface and a constraint (Step 4).

(`Add`, `GetAll` and `Remove` compile fine because they never need to know anything about `T`.)

## Step 7 — Why `new Store<string>()` must NOT compile

In `Program.cs` this line is left commented out and marked **must NOT compile**:

```csharp
// var words = new Store<string>();   // must NOT compile
```

When it is un-commented, `dotnet build` fails with:

```text
Program.cs(68,23): error CS0311: The type 'string' cannot be used as type parameter 'T' in the generic type
or method 'Store<T>'. There is no implicit reference conversion from 'string' to 'Generics.IHasId'.
```

**Why:** `Store<T>` is declared `where T : IHasId`. That constraint is a promise the compiler checks at
*every place the generic type is used*: whatever goes into `T` must implement `IHasId`. `string` does not
implement `IHasId` (it has no `Id`), so `Store<string>` breaks the promise and is rejected at **compile
time**. This is exactly what we want: `Store<T>` relies on `item.Id` (as the dictionary key, in `GetById`,
in the duplicate check), so a type without an `Id` would make no sense in it. The constraint turns a
mistake that would otherwise be a bug at run time into a red line in the editor — the same constraint that
let `GetById` compile in Step 4 also protects the store from wrong types.

`Store<Student>` and `Store<Course>` compile because both classes implement `IHasId`. The same rule applies
to the extension methods: `FindById` and `ToIdDictionary` work on a plain `List<Course>`, but would not
compile on a `List<string>`.

### Step 7 output

```text
=== Get one of each by id ===
students.GetById(3)   -> #3 Omar Khaled
courses.GetById(102)  -> #102 OOP in .NET (2000 EGP)
students.GetById(99)  -> null

=== Add a student with an id that already exists ===
Caught: Cannot add Student with Id 2: an item with this Id already exists in the store.
Still 5 students; id 2 is still #2 Mona Hassan

=== Page 2 (size 2) of the students ===
  #3 Omar Khaled
  #4 Sara Youssef

=== Invalid page numbers throw ===
Caught: pageNumber ('0') must be greater than or equal to '1'. (Parameter 'pageNumber')
Actual value was 0.

=== FindById on a plain List<Course> (not a Store) ===
courseList.FindById(202) -> #202 ASP.NET Core (2500 EGP)
courseList.FindById(999) -> null

=== ToIdDictionary ===
byId[203] -> #203 Design Patterns (1700 EGP)   (Count = 3)
```

> Note on paging a store: `GetAll()` returns the dictionary's values. A `Dictionary` does not officially
> promise any order; in practice it returns items in the order they were added as long as nothing was
> removed, which is why page 2 is students 3 and 4 here. If a guaranteed order were needed, the store could
> keep a `SortedDictionary<int, T>` instead (ordered by Id, O(log n) per operation).

## Design decisions

- **`GetAll()` returns `IReadOnlyCollection<T>`** — the caller can loop over the items and read `Count`, but the
  type has no `Add`/`Remove`/`Clear`, so only the store can change its contents. It returns the dictionary's
  `Values` directly (a live read-only view, no copy); even a cast to `ICollection<T>` cannot change it —
  `Add`/`Remove` on that view throw `NotSupportedException`.
- **Duplicate Id → `InvalidOperationException`** with a message that names the type and the Id. The store is
  left unchanged.
- **`GetById` returns `T?`** — `null` for classes when the id is not found (Step 1's "returns null"), `default`
  in general because `T` is only constrained to `IHasId`.
- **`Page` validates eagerly, iterates lazily** — the argument checks run as soon as `Page` is called (a plain
  method), and the actual paging is a separate `yield return` iterator that stops reading the source with
  `yield break` once the page is full.
- **No LINQ** is used in `Store<T>` or `EnumerableExtensions` — only `foreach`, `yield`, `Dictionary` and `List`.

## Last question — what is the common name for the kind of class built in Steps 4 and 5?

A **Generic Repository** (the *Repository pattern* made generic): one class, `Store<T>` — in real projects
usually called `Repository<T>` / `IRepository<T>` — that gives the same CRUD operations (`Add`, `GetById`,
`GetAll`, `Remove`) for **any entity type** that has an Id, constrained with `where T : IEntity` / `IHasId`
(the interface is often called `IEntity`). Because it keeps the data in memory (a `Dictionary`), it is
specifically an **in-memory generic repository** — the kind often used as a fake repository in tests in
place of one that talks to a database (e.g. through Entity Framework Core).
