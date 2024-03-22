using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Services.Query.GetList;

public class GetListQueryValidator : AbstractValidator<GetListQuery>
{
    public GetListQueryValidator()
    {
        RuleFor(t => t.limit).GreaterThan(0).When(t => t.limit.HasValue);
        RuleFor(t => t.offset).GreaterThan(0).When(t => t.offset.HasValue);
        RuleFor(t => t.nameText).MaximumLength(100);
    }
}
