namespace Generics;

public class Course
{
    public Course(int id, string title, decimal price)
    {
        Id = id;
        Title = title;
        Price = price;
    }

    public int Id { get; }

    public string Title { get; }

    public decimal Price { get; }

    public override string ToString() => $"#{Id} {Title} ({Price} EGP)";
}
