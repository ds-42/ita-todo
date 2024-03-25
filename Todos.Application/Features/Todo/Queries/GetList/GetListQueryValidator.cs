using FluentValidation;

namespace Todos.Application.Features.Todo.Queries.GetList;

public class GetListQueryValidator : AbstractValidator<GetListQuery>
{
    public GetListQueryValidator()
    {
        RuleFor(t => t.Limit).GreaterThan(0).When(t => t.Limit.HasValue);
        RuleFor(t => t.Offset).GreaterThan(0).When(t => t.Offset.HasValue);
        RuleFor(t => t.Predicate).MaximumLength(100);
        RuleFor(t => t.OwnerId).GreaterThan(0).When(t => t.Limit.HasValue);
    }
}
