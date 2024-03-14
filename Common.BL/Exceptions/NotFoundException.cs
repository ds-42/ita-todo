using System.Net;
using System.Text.Json;

namespace Common.BL.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException(object filter) : base(JsonSerializer.Serialize(filter), HttpStatusCode.NotFound)
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

