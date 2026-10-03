namespace Generics;

/// <summary>
/// Generic extension methods on IEnumerable&lt;T&gt; — written with foreach / yield return, no LINQ.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Returns only the items of one page (page 1 is the first page). Lazy: it stops reading the source as
    /// soon as the page is complete.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">pageNumber or pageSize is less than 1.</exception>
    public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        // Validate here, not inside the iterator: an iterator method runs nothing until the first
        // MoveNext(), so invalid numbers would only throw later, far from the call. This way they throw now.
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        // long: (pageNumber - 1) * pageSize can overflow int for very large page numbers.
        var toSkip = (long)(pageNumber - 1) * pageSize;
        return PageIterator(source, toSkip, pageSize);
    }

    private static IEnumerable<T> PageIterator<T>(IEnumerable<T> source, long toSkip, int pageSize)
    {
        long index = 0;
        var returned = 0;

        foreach (var item in source)
        {
            if (index++ < toSkip)
                continue;

            yield return item;

            if (++returned == pageSize)
                yield break; // page is full — do not read the rest of the source
        }
    }

    /// <summary>Returns the item with that id, or default (null for classes) if there is none.</summary>
    public static T? FindById<T>(this IEnumerable<T> source, int id) where T : IHasId
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (var item in source)
        {
            if (item.Id == id)
                return item;
        }
        return default;
    }

    /// <summary>Copies the items into a dictionary keyed by Id and returns it as read-only.</summary>
    /// <exception cref="InvalidOperationException">Two items have the same Id.</exception>
    public static IReadOnlyDictionary<int, T> ToIdDictionary<T>(this IEnumerable<T> source) where T : IHasId
    {
        ArgumentNullException.ThrowIfNull(source);

        var result = new Dictionary<int, T>();
        foreach (var item in source)
        {
            if (!result.TryAdd(item.Id, item))
                throw new InvalidOperationException(
                    $"Cannot build the dictionary: the Id {item.Id} appears more than once.");
        }
        return result;
    }
}
