using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Exceptions
{
    public class InvalidUserException : System.Exception
    {
        public InvalidUserException() : base("Invalid user") { }
    }
}
