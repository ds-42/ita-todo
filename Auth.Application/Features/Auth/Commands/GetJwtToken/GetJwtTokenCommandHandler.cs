using Auth.Application.Dto;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain.Auth;
using Common.Domain.Users;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Services.Utils;

namespace Auth.Application.Features.Auth.Commands.GetJwtToken;

public class GetJwtTokenCommandHandler : IRequestHandler<GetJwtTokenCommand, JwtTokenDto>
{
    private readonly IConfiguration _config;
    private readonly IRepository<RefreshToken> _refreshTokens;
    private readonly IRepository<ApplicationUser> _users;

    public GetJwtTokenCommandHandler(
        IConfiguration config,
        IRepository<RefreshToken> refreshTokens,
        IRepository<ApplicationUser> users)
    {
        _config = config;
        _refreshTokens = refreshTokens;
        _users = users;
    }

    public async Task<JwtTokenDto> Handle(GetJwtTokenCommand command, CancellationToken cancellationToken)
    {
        var user = await _users.SingleOrDefaultAsync(t => t.Login == command.Login.Trim(), cancellationToken);
        if (user == null)
        {
            throw new BadRequestException("Invalid login or password");
        }


        if (PasswordHashUtils.Verify(command.Password, user.Password) == false)
        {
            throw new FormatException();
        }

        var claims = new List<Claim>()
        {
            new (ClaimTypes.Name, user.Login),
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var dateExpire = DateTime.UtcNow.AddDays(5); // AddMinutes(5);

        var tokenDescriptor = new JwtSecurityToken(_config["Jwt:Issuer"]!, _config["Jwt:Audience"]!, claims,
            expires: dateExpire,
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor)!;

        var refreshToken = await _refreshTokens.AddAsync(new RefreshToken()
        {
            ApplicationUserId = user.Id
        }, cancellationToken);

        return new JwtTokenDto()
        {
            JwtToken = token,
            RefreshToken = refreshToken.Id,
            Expire = dateExpire,
        };
    }
}
