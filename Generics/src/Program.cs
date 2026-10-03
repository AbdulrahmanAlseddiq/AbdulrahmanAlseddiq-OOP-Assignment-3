using Generics;

// Step 1 — a store for students.
var students = new StudentStore();
students.Add(new Student(1, "Ahmed"));
students.Add(new Student(2, "Mona"));

Console.WriteLine($"GetById(2): {students.GetById(2)}");
Console.WriteLine($"GetById(9): {students.GetById(9)?.ToString() ?? "null"}");
Console.WriteLine($"Remove(1): {students.Remove(1)}");
foreach (var s in students.GetAll())
    Console.WriteLine($"  {s}");
