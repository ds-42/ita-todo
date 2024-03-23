namespace Common.Application.Abstractions;

public interface ICurrentUserService
{
    int UserId { get; }

    string[] UserRoles { get; }

    bool IsAdmin { get; }
}
