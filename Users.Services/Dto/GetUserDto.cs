using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Services.Dto;

public class GetUserDto
{
    public int Id { get; set; }
    public string Login { get; set; } = default!;
}
