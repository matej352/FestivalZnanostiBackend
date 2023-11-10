using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Services
{
    public interface IAuthService
    {
        public Task<int> registerAccount(RegisterDto registerDto);

        public Task<AccountDto> loginAccount(LoginDto loginDto);

        public Task<bool> isEmailUnique(string email);
    }
}
