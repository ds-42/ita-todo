using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Services.Command.CreateUser;

public class CreateUserCommand
{
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
}
