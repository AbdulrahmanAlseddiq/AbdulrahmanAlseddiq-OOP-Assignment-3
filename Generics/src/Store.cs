namespace Generics;

/// <summary>
/// One in-memory store for any entity that has an Id.
/// Items are kept in a Dictionary keyed by Id, so GetById, Remove and the duplicate check are O(1)
/// on average — no loop over the items.
/// </summary>
public class Store<T> where T : IHasId
{
    private readonly Dictionary<int, T> _items = new();

    public int Count => _items.Count;

    /// <exception cref="InvalidOperationException">An item with the same Id is already in the store.</exception>
    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!_items.TryAdd(item.Id, item))
            throw new InvalidOperationException(
                $"Cannot add {typeof(T).Name} with Id {item.Id}: an item with this Id already exists in the store.");
    }

    // Returns default (null for classes) if not found.
    public T? GetById(int id) =>
        _items.TryGetValue(id, out var item) ? item : default;

    // IReadOnlyCollection<T>: the caller can loop and read Count, but the type has no Add / Remove / Clear.
    // The Values collection is a read-only view of the dictionary — no copy is made, and even if a caller
    // casts it to ICollection<T>, Add/Remove throw NotSupportedException. Only the store can change its items.
    public IReadOnlyCollection<T> GetAll() => _items.Values;

    public bool Remove(int id) => _items.Remove(id);
}
