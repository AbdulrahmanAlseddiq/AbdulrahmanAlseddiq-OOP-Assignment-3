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
