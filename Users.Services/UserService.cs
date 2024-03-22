using AutoMapper;
using Common.Api.Extensions;
using Common.Api.Services;
using Common.BL.Exceptions;
using Common.Domain;
using Common.Domain.Exceptions;
using Common.Repositories;
using Users.Services.Dto;
using Users.Services.Utils;

namespace Users.Services;

public class UserService : IUserService
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public UserService(
        IRepository<ApplicationUser> userRepositiry, 
        ICurrentUserService currentUser, 
        IMapper mapper) 
    {
        _userRepository = userRepositiry;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    protected async Task<ApplicationUser> GetUserAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (user == null)
            throw new InvalidUserException(id);

        return user;
    }

    public async Task<GetUserDto?> GetByIdOrDefaultAsync(int id, CancellationToken cancellationToken = default)
    {
        return _mapper.Map<GetUserDto>(await _userRepository.SingleOrDefaultAsync(t => t.Id == id, cancellationToken));
    }


    public async Task<GetUserDto> UpdateAsync(int id, UpdateUserDto user, CancellationToken cancellationToken = default)
    {
        _currentUser.TestAccess(id);

        var item = await GetUserAsync(id, cancellationToken);

        item.Login = user.Login;
        item.Password = PasswordHashUtils.Hash(user.Password);

        return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(item, cancellationToken));
    }

    public async Task<GetUserDto> ChangePasswordAsync(int id, string password, CancellationToken cancellationToken = default)
    {
        _currentUser.TestAccess(id);

        var item = await GetUserAsync(id, cancellationToken);

        item.Password = PasswordHashUtils.Hash(password);

        return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(item, cancellationToken));
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _userRepository.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (item == null)
            throw NotFoundException.Create(new { Id = id });

        return await _userRepository.DeleteAsync(item, cancellationToken);
    }
}
