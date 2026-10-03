namespace Generics;

public class CourseStore
{
    private readonly List<Course> _courses = new();

    public void Add(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);
        _courses.Add(course);
    }

    // Returns null if not found.
    public Course? GetById(int id)
    {
        foreach (var course in _courses)
        {
            if (course.Id == id)
                return course;
        }
        return null;
    }

    public List<Course> GetAll() => _courses;

    public bool Remove(int id)
    {
        var course = GetById(id);
        return course is not null && _courses.Remove(course);
    }
}
