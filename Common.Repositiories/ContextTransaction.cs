using Common.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Persistence;

public class ContextTransaction : IContextTransaction
{
    protected readonly IDbContextTransaction _dbContextTransaction;
    public void Dispose()
    {
        _dbContextTransaction.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _dbContextTransaction.DisposeAsync();
    }

    public ContextTransaction(IDbContextTransaction contextTransaction) 
    {
        _dbContextTransaction = contextTransaction;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _dbContextTransaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await _dbContextTransaction.RollbackAsync(cancellationToken);
    }
}
