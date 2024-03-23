using System.Net;

namespace Common.Application.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException(object filter) : base(filter, HttpStatusCode.NotFound)
    {
    }

    public static NotFoundException Create(object filter, string note = "Not found by filter")
    {
        return new NotFoundException(new
        {
            Filter = filter,
            Message = note
        });
    }

}

