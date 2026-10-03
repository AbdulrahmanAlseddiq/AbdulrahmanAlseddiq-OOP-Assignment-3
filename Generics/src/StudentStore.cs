namespace Generics;

public class StudentStore
{
    private readonly List<Student> _students = new();

    public void Add(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        _students.Add(student);
    }

    // Returns null if not found.
    public Student? GetById(int id)
    {
        foreach (var student in _students)
        {
            if (student.Id == id)
                return student;
        }
        return null;
    }

    public List<Student> GetAll() => _students;

    public bool Remove(int id)
    {
        var student = GetById(id);
        return student is not null && _students.Remove(student);
    }
}
