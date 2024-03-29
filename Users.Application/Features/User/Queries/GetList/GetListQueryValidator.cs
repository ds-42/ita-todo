﻿using FluentValidation;

namespace Users.Application.Features.User.Queries.GetList;

public class GetListQueryValidator : AbstractValidator<GetListQuery>
{
    public GetListQueryValidator()
    {
        RuleFor(t => t.Limit).GreaterThan(0).When(t => t.Limit.HasValue);
        RuleFor(t => t.Offset).GreaterThan(0).When(t => t.Offset.HasValue);
        RuleFor(t => t.Predicate).MaximumLength(100);
    }
}
