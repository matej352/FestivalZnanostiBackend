using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Middlewares.UserContext;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using FestivalZnanostiApi.Utils;
using System.Text;

namespace FestivalZnanostiApi.Services.impl
{
    public class AuthService : IAuthService
    {

        private readonly UserContext _userContext;

        private readonly IAuthRepository _authRepository;

        private readonly IAccountService _accountService;


        public AuthService(UserContext userContext, IAuthRepository authRepository, IAccountService accountService)
        {
            _userContext = userContext;
            _authRepository = authRepository;
            _accountService = accountService;
        }



        public async Task<int> registerAccount(RegisterDto registerDto)
        {

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                throw new Exception("Password and confirm password mismatch!");
            }

            var emailUnque = await isEmailUnique(registerDto.Email);
            if (!emailUnque)
            {
                throw new Exception($"Account with email {registerDto.Email} already existst");
            }

            var newAccountId = await _accountService.CreateAccount(registerDto);


            return newAccountId;

        }





        public async Task<AccountDto> loginAccount(LoginDto loginDto)
        {
            var account = await _accountService.GetAccountByEmail(loginDto.Email);

            if (account is null)
            {
                throw new Exception("Neispravan email ili lozinka!");
            }
            var passwordHashingHandler = new PasswordHashingHandler(loginDto.Password);


            if (!passwordHashingHandler.VerifyPasswordHash(Convert.FromBase64String(account.Password), Convert.FromBase64String(account.Salt)))
            {
                throw new Exception("Neispravan email ili lozinka!");
            }

            return account.AsAccountDto();
        }





        public async Task<bool> isEmailUnique(string email)
        {
            var accountId = await _authRepository.FindByEmail(email);

            if (accountId == null)
            {
                return true;
            }
            return false;
        }


    }
}
