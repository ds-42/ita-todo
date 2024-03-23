using Common.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public string[] UserRoles => _httpContextAccessor.HttpContext.User.Claims
        .Where(t => t.Type == ClaimTypes.Role)
        .Select(t => t.Value)
        .ToArray();

    public bool IsAdmin => UserRoles.Contains("Admin");

}
