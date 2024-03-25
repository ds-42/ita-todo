namespace Common.Application.Abstractions.Persistence;

public interface IContextTransactionCreator
{
    Task<IContextTransaction> CreateTransactionAsync(CancellationToken cancelationToken);
}
