using Auth.Application.Dto;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Services.Utils;

namespace Auth.Application;

public class AuthService : IAuthService
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IRepository<RefreshToken> _refreshTokens;
    private readonly IConfiguration _config;

    public AuthService(
        IRepository<ApplicationUser> userRepository,
        IRepository<RefreshToken> refreshTokens,
        IConfiguration config)
    {
        _config = config;

        _userRepository = userRepository;
        _refreshTokens = refreshTokens;
    }

    public async Task<JwtTokenDto> GetJwtTokenAsync(AuthDto userDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.SingleOrDefaultAsync(t => t.Login == userDto.Login.Trim(), cancellationToken);
        if (user == null)
        {
            throw new BadRequestException("Invalid login or password");
        }


        if (PasswordHashUtils.Verify(userDto.Password, user.Password) == false)
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

    public async Task<JwtTokenDto> GetJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenFromDb = await _refreshTokens.SingleOrDefaultAsync(t => t.Id == refreshToken, cancellationToken);
        if (refreshTokenFromDb == null)
        {
            throw new ForbiddenException();
        }

        var user = await _userRepository.SingleAsync(t => t.Id == refreshTokenFromDb.ApplicationUserId, cancellationToken);
        if (user == null)
        {
            throw new BadRequestException("Invalid login or password");
        }

        var claims = new List<Claim>()
        {
            new (ClaimTypes.Name, user.Login),
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var dateExpire = DateTime.UtcNow.AddMinutes(5);

        var tokenDescriptor = new JwtSecurityToken(_config["Jwt:Issuer"]!, _config["Jwt:Audience"]!, claims,
            expires: dateExpire,
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor)!;

        return new JwtTokenDto()
        {
            JwtToken = token,
            RefreshToken = refreshTokenFromDb.Id,
            Expire = dateExpire,
        };
    }
}
