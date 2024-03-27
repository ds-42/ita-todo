using FluentValidation;

namespace Todos.Application.Extensions;

// https://docs.fluentvalidation.net/en/latest/custom-validators.html

public static class TodoValidatorExtensions
{
    public static IRuleBuilderOptions<T, int> AsTodoId<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Invalid todo Id");
    }

    public static IRuleBuilderOptions<T, int> AsTodoOwnerId<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid owner Id");
    }

    public static IRuleBuilderOptions<T, string> AsTodoLabel<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MinimumLength(5)
            .MaximumLength(100)
            .WithMessage("Invalid Label");
    }
}
