using Common.Application.Abstractions.Persistence;
using MediatR;

namespace Common.Application.Behaviors;

public class ContextTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public readonly IContextTransactionCreator _creator;
    public ContextTransactionBehavior(IContextTransactionCreator creater) 
    {
        _creator = creater;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
//        if (next.Target is IQueryHandler<TRequest, TResponse>)
//            return await next();

        // faq: Транзакция выполняется в том числе для query, не излишне ли это?
        using (var transaction = await _creator.CreateTransactionAsync(cancellationToken)) 
        {
            try 
            {
                var result = await next();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception) 
            {
                await transaction.RollbackAsync(CancellationToken.None);
                throw;
            }
        }
    }
}
