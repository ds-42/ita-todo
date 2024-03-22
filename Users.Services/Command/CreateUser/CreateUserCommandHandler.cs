using AutoMapper;
using Common.BL.Exceptions;
using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Users.Services.Dto;
using Users.Services.Utils;

namespace Users.Services.Command.CreateUser;

public class CreateUserCommandHandler
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IRepository<ApplicationUserRole> _userRoles;
    private readonly MemoryCache _cache;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IRepository<ApplicationUser> userRepositiry,
        IRepository<ApplicationUserRole> userRoles,
        UsersMemoryCache cache,
        IMapper mapper) 
    {
        _userRepository = userRepositiry;
        _userRoles = userRoles;
        _mapper = mapper;
        _cache = cache.Cache;
    }

    public async Task<GetUserDto> RunAsync(CreateUserCommand dto, CancellationToken cancellationToken = default)
    {
        var login = dto.Login.Trim();

        if (await _userRepository.SingleOrDefaultAsync(t => t.Login == login, cancellationToken) != null)
        {
            throw new BadRequestException("Invalid login or password");
        }

        var userRoles = (await _userRoles.SingleOrDefaultAsync(t => t.Name == "Client", cancellationToken))!;

        var user = new ApplicationUser()
        {
            Login = login,
            Password = PasswordHashUtils.Hash(dto.Password),
            Roles = new[] { new ApplicationUserApplicationRole() { ApplicationUserRoleId = userRoles.Id } },
        };

        _cache.Clear();
        return _mapper.Map<GetUserDto>(await _userRepository.AddAsync(user, cancellationToken));
    }

}
