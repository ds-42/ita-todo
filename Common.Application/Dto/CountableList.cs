namespace Common.Application.Dto;

public class CountableList<T>
{
    public int Count { get; init; }

    public IReadOnlyCollection<T> Items { get; init; } = default!;
}
