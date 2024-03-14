using Users.Services.Dto;

namespace Users.Services
{
    internal interface IAuthService
    {
        public Task<string> GetJwtToken(AuthDto userDto);
    }
}
