namespace RefactoringLab.Part03.Students;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public static class StudentCatalog
{
    // Lazy: each student is created only when the caller asks for the next one.
    // If the caller stops after 3, the other 999,997 are never created.
    public static IEnumerable<Student> GetAllStudents()
    {
        for (var i = 1; i <= 1_000_000; i++)
            yield return new Student { Id = i, Name = $"Student {i}" };
    }
}
