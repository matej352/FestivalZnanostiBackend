using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Services
{
    public interface IAccountService
    {
        public Task<int> CreateAccount(RegisterDto registerDto);

        public Task<Account?> GetAccountByEmail(string email);
    }
}
