using FluentValidation;
using Todos.Application.Extensions;

namespace Todos.Application.Features.Todo.Queries.GetById;

public class GetByIdQueryValidator : AbstractValidator<GetByIdQuery>
{
    public GetByIdQueryValidator()
    {
        RuleFor(t => t.Id).AsTodoId();
    }
}
