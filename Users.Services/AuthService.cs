using Common.BL.Exceptions;
using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Users.Services.Dto;
using Users.Services.Utils;

namespace Users.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserRole> _roleRepository;
    private readonly IConfiguration _config;

    public AuthService(IRepository<User> userRepository, IRepository<UserRole> roleRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
        _roleRepository = roleRepository;
    }

    public async Task<string> GetJwtToken(AuthDto userDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.SingleOrDefaultAsync(t => t.Login == userDto.Login.Trim(), cancellationToken);
        if (user == null)
        {
            throw new BadRequestException("Invalid login or password");
        }

        if(PasswordHashUtils.Verify(userDto.Password, user.Password) == false) 
        {
            throw new FormatException();
        }

        var role = await _roleRepository.SingleOrDefaultAsync(t => t.Id == user.RoleId, cancellationToken);

        var claims = new List<Claim>()
        {
            new (ClaimTypes.Name, user.Login),
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Role, role!.Name),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(_config["Jwt:Issuer"]!, _config["Jwt:Audience"]!, claims, 
            expires: DateTime.UtcNow.AddMinutes(5), 
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor)!;
        return token;
    }
}
