namespace Generics;

/// <summary>Anything that can be stored in <see cref="Store{T}"/> must have a read-only integer Id.</summary>
public interface IHasId
{
    int Id { get; }
}
