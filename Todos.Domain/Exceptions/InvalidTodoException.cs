using Common.Application.Exceptions;
namespace Todos.Domain;

public class InvalidTodoException : NotFoundException
{
    public InvalidTodoException(int id) : base(new
    {
        Id = id,
        Message = "Invalid todo Id",
    })
    { }
}

