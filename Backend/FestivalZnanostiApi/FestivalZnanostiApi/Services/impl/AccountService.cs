using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using FestivalZnanostiApi.Utils;

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

        public async Task<AccountDto> GetAccount(int id)
        {
            var account = await _repository.GetAccount(id);
            if (account == null)
            {
                throw new Exception($"Account does not exist!");
            }
            return account.AsAccountDto();
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _repository.GetAccountByEmail(email);
        }

        public async Task UserForgotPassword(int accountId, string newPassword, string confirmNewPassword)
        {
            var account = await _repository.GetAccount(accountId);
            if (account == null)
            {
                throw new Exception($"Account does not exist!");
            }

            if (newPassword != confirmNewPassword)
            {
                throw new Exception("New password and confirm new password mismatch!");
            }

            await _repository.ChangePassword(accountId, newPassword);

        }

        public async Task ChangeMyPassword(ChangePasswordDto changePasswordDto)
        {

            if (changePasswordDto.OldPassword is null)
            {
                throw new Exception("Trenutna lozinka je obavezna!");
            }

            var account = await _repository.GetAccount(changePasswordDto.AccountId);
            if (account == null)
            {
                throw new Exception($"Account does not exist!");
            }

            //check old password
            var passwordHashingHandler = new PasswordHashingHandler(changePasswordDto.OldPassword);


            if (!passwordHashingHandler.VerifyPasswordHash(Convert.FromBase64String(account.Password), Convert.FromBase64String(account.Salt)))
            {
                throw new Exception("Neispravna trenutna lozinka!");
            }


            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            {
                throw new Exception("New password and confirm new password mismatch!");
            }

            await _repository.ChangePassword(changePasswordDto.AccountId, changePasswordDto.NewPassword);

        }


    }
}
