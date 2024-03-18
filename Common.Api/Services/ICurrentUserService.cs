using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.Api.Services;

public interface ICurrentUserService
{
    int UserId { get; }

    string[] UserRoles { get; }

    bool IsAdmin { get; }

    void TestAccess(int userId);
}
