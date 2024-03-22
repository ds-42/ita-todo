using Common.Api.Services;
using Common.BL.Exceptions;

namespace Common.Api.Extensions;

public static class CurrentUserExtension
{
    public static void TestAccess(this ICurrentUserService user, int userId)
    {
        if (user.IsAdmin || user.UserId == userId)
            return;

        throw new AccessDeniedException();
    }
}
