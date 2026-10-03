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
