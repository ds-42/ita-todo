using AutoMapper;
using Common.BL.Exceptions;
using Common.Domain;
using Common.Repositories;
using Users.Services.Dto;
using Users.Services.Utils;

namespace Users.Services.Command.CreateUser;

public class CreateUserCommandHandler
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IRepository<ApplicationUserRole> _userRoles;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IRepository<ApplicationUser> userRepositiry,
        IRepository<ApplicationUserRole> userRoles,
        IMapper mapper) 
    {
        _userRepository = userRepositiry;
        _userRoles = userRoles;
        _mapper = mapper;
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

        return _mapper.Map<GetUserDto>(await _userRepository.AddAsync(user, cancellationToken));
    }

}
