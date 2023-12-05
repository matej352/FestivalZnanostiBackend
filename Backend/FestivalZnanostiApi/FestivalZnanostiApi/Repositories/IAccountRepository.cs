using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface IAccountRepository
    {
        public Task<int> CreateAccount(RegisterDto registerDto);


        public Task<Account?> GetAccountByEmail(string email);


        public Task<Account?> GetAccount(int id);
    }
}
