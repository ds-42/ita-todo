using Common.Application.Abstractions;
using Common.Application.Exceptions;

namespace Common.Application.Extensions;

public static class CurrentUserExtension
{
    public static void TestAccess(this ICurrentUserService user, int userId)
    {
        if (user.IsAdmin || user.UserId == userId)
            return;

        throw new AccessDeniedException();
    }
}
