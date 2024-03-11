using Common.Api.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

