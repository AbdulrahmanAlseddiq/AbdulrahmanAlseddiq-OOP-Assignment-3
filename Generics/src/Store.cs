namespace Generics;

public class Store<T>
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    // Step 3: this is where it breaks. The line below does NOT compile:
    //   error CS1061: 'T' does not contain a definition for 'Id' and no accessible extension method 'Id'
    //   accepting a first argument of type 'T' could be found
    // It is left commented out so the project still builds — see Generics/Answers.md (Step 3).
    public T? GetById(int id)
    {
        foreach (var item in _items)
        {
            // if (item.Id == id)
            //     return item;
        }
        return default;
    }

    public List<T> GetAll() => _items;

    public bool Remove(int id)
    {
        var item = GetById(id);
        return item is not null && _items.Remove(item);
    }
}
