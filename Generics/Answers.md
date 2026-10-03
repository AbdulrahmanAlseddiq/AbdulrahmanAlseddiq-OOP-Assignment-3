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
