using Common.Application.Exceptions;

namespace Todos.Application.Exceptions;

public class InvalidTodoException : NotFoundException
{
    public InvalidTodoException(int id) : base(new
    {
        Id = id,
        Message = "Invalid todo Id",
    })
    { }
}

