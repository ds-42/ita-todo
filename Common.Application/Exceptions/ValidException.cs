using FluentValidation.Results;
using System.Net;

namespace Common.Application.Exceptions;

public class ValidException : HttpException
{
    public readonly List<ValidationFailure> errors;
    public ValidException(List<ValidationFailure> errors) : base(errors, HttpStatusCode.BadRequest)
    {
        this.errors = errors;
    }
}
