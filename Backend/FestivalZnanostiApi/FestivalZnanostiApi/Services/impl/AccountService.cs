using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;

namespace FestivalZnanostiApi.Services.impl
{
    public class AccountService : IAccountService
    {

        private readonly IAccountRepository _repository;



        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }


        public async Task<int> CreateAccount(RegisterDto registerDto)
        {
            return await _repository.CreateAccount(registerDto);
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _repository.GetAccountByEmail(email);
        }
    }
}
