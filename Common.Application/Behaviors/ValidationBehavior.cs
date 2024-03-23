﻿using Common.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Common.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IValidator<TRequest> _validator;
    public ValidationBehavior(IValidator<TRequest> validator) 
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidException(result.Errors);
        }

        return await next();
    }
}
