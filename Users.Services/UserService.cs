using AutoMapper;
using Common.BL.Exceptions;
using Common.Domain;
using Common.Repositories;
using Users.Services.Dto;
using Users.Services.Utils;

namespace Users.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IRepository<User> userRepositiry, IMapper mapper) 
    {
        _userRepository = userRepositiry;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<GetUserDto>> GetItemsAsync(int offset = 0, int limit = 10, string nameText = "", CancellationToken cancellationToken = default) 
    {
        return _mapper.Map <IReadOnlyCollection<GetUserDto>>(await _userRepository.GetItemsAsync(
            offset, 
            limit, 
            string.IsNullOrWhiteSpace(nameText)
                ? null 
                : t => t.Login.Contains(nameText, StringComparison.InvariantCultureIgnoreCase), 
            t => t.Id, null, cancellationToken));
    }

    public async Task<GetUserDto?> GetByIdOrDefaultAsync(int id, CancellationToken cancellationToken = default)
    {
        return _mapper.Map<GetUserDto>(await _userRepository.SingleOrDefaultAsync(t => t.Id == id, cancellationToken));
    }

    public async Task<GetUserDto> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var login = dto.Login.Trim();

        if (await _userRepository.SingleOrDefaultAsync(t => t.Login == login, cancellationToken) != null)
        {
            throw new BadRequestException("Invalid login or password");
        }

        var user = new User()
        {
            Login = login,
            Password = PasswordHashUtils.Hash(dto.Password),
            RoleId = 1,
        };

        return _mapper.Map<GetUserDto>(await _userRepository.AddAsync(user, cancellationToken));
    }


    public async Task<GetUserDto?> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var item = await GetByIdOrDefaultAsync(user.Id, cancellationToken);

        if (item == null) 
            return null;

        return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(user, cancellationToken));
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _userRepository.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (item == null)
            throw NotFoundException.Create(new { Id = id });

        return await _userRepository.DeleteAsync(item, cancellationToken);
    }
}
