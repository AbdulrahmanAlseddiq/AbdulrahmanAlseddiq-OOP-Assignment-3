using Generics;

// Step 4 — one generic store replaces StudentStore and CourseStore.
var students = new Store<Student>();
students.Add(new Student(1, "Ahmed"));
students.Add(new Student(2, "Mona"));

Console.WriteLine($"GetById(2): {students.GetById(2)}");
Console.WriteLine($"GetById(9): {students.GetById(9)?.ToString() ?? "null"}");
Console.WriteLine($"Remove(1): {students.Remove(1)}");
foreach (var s in students.GetAll())
    Console.WriteLine($"  {s}");

var courses = new Store<Course>();
courses.Add(new Course(10, "C# Basics", 1500m));
courses.Add(new Course(20, "OOP", 2000m));
Console.WriteLine($"GetById(20): {courses.GetById(20)}");

// Step 5 — fast and safe.
try
{
    students.Add(new Student(2, "Duplicate Mona"));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Duplicate rejected: {ex.Message}");
}

IReadOnlyCollection<Student> all = students.GetAll();
// all.Add(...)   ← does not compile: IReadOnlyCollection<T> has no Add.
Console.WriteLine($"Count via read-only view: {all.Count}");

// Step 6 — generic extension methods on any IEnumerable<T>.
var numbers = new List<Course>
{
    new(1, "A", 1m), new(2, "B", 2m), new(3, "C", 3m), new(4, "D", 4m), new(5, "E", 5m),
};
foreach (var c in numbers.Page(pageNumber: 3, pageSize: 2))
    Console.WriteLine($"  page 3 (size 2): {c}");
Console.WriteLine($"FindById(4): {numbers.FindById(4)}");
Console.WriteLine($"ToIdDictionary()[5]: {numbers.ToIdDictionary()[5]}");
