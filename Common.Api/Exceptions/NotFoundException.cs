using System.Net;
using System.Text.Json;

namespace Common.Api.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException(object filter) : base(JsonSerializer.Serialize(filter), HttpStatusCode.NotFound)
    {
    }
}

public abstract partial class Exceptions
{
    public static NotFoundException NotFound(object filter, string note = "Not found by filter")
        => new NotFoundException(new 
        {
            Filter = filter,
            Message = note
        });
}