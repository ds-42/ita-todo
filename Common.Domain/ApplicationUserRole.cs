using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain;

public class ApplicationUserRole
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<ApplicationUserApplicationRole> Users { get; set; } = default!;
}
