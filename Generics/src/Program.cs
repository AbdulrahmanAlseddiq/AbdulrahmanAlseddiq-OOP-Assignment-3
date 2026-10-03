using Generics;

// ===== Step 7 — prove it =====

// 5 students in a Store<Student>, 3 courses in a Store<Course>.
var students = new Store<Student>();
students.Add(new Student(1, "Ahmed Ali"));
students.Add(new Student(2, "Mona Hassan"));
students.Add(new Student(3, "Omar Khaled"));
students.Add(new Student(4, "Sara Youssef"));
students.Add(new Student(5, "Youssef Adel"));

var courses = new Store<Course>();
courses.Add(new Course(101, "C# Fundamentals", 1500m));
courses.Add(new Course(102, "OOP in .NET", 2000m));
courses.Add(new Course(103, "Generics & Collections", 1800m));

Console.WriteLine("=== Get one of each by id ===");
Console.WriteLine($"students.GetById(3)   -> {students.GetById(3)}");
Console.WriteLine($"courses.GetById(102)  -> {courses.GetById(102)}");
Console.WriteLine($"students.GetById(99)  -> {students.GetById(99)?.ToString() ?? "null"}");
Console.WriteLine();

Console.WriteLine("=== Add a student with an id that already exists ===");
try
{
    students.Add(new Student(2, "Another Mona"));
    Console.WriteLine("ERROR: the duplicate was accepted!");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}
Console.WriteLine($"Still {students.Count} students; id 2 is still {students.GetById(2)}");
Console.WriteLine();

Console.WriteLine("=== Page 2 (size 2) of the students ===");
foreach (var student in students.GetAll().Page(pageNumber: 2, pageSize: 2))
    Console.WriteLine($"  {student}");
Console.WriteLine();

Console.WriteLine("=== Invalid page numbers throw ===");
try
{
    students.GetAll().Page(pageNumber: 0, pageSize: 2);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}
Console.WriteLine();

Console.WriteLine("=== FindById on a plain List<Course> (not a Store) ===");
var courseList = new List<Course>
{
    new(201, "SQL Server", 1200m),
    new(202, "ASP.NET Core", 2500m),
    new(203, "Design Patterns", 1700m),
};
Console.WriteLine($"courseList.FindById(202) -> {courseList.FindById(202)}");
Console.WriteLine($"courseList.FindById(999) -> {courseList.FindById(999)?.ToString() ?? "null"}");
Console.WriteLine();

Console.WriteLine("=== ToIdDictionary ===");
IReadOnlyDictionary<int, Course> byId = courseList.ToIdDictionary();
Console.WriteLine($"byId[203] -> {byId[203]}   (Count = {byId.Count})");

// var words = new Store<string>();   // must NOT compile — string does not implement IHasId (error CS0311). See Answers.md (Step 7).
