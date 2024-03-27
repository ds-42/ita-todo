using FluentValidation;
using System.Linq.Expressions;

namespace Todos.Application.Features.Todo;

public abstract class TodoValidator<T> : AbstractValidator<T>
{
    public IRuleBuilderOptions<T, int> RuleForId(Expression<Func<T, int>> predicate) => RuleFor(predicate)
        .NotNull()
        .GreaterThan(0)
        .WithMessage("Invalid todo Id");

    public IRuleBuilderOptions<T, int> RuleForOwnerId(Expression<Func<T, int>> predicate) => RuleFor(predicate)
        .NotNull()
        .GreaterThan(0)
        .WithMessage("Invalid owner Id");

    public IRuleBuilderOptions<T, string> RuleForLabel(Expression<Func<T, string>> predicate) => RuleFor(predicate)
        .NotNull()
        .MinimumLength(5)
        .MaximumLength(100)
        .WithMessage("Invalid Label");
}
