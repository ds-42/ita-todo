using Common.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Persistence;

public class ContextTransactionCreator : IContextTransactionCreator
{
    protected readonly ApplicationDbContext _context;

    public ContextTransactionCreator(ApplicationDbContext dbContext) 
    {
        _context = dbContext;
    }

    public async Task<IContextTransaction> CreateTransactionAsync(CancellationToken cancelationToken)
    {
        return new ContextTransaction(await _context.Database.BeginTransactionAsync(cancelationToken));
    }
}
