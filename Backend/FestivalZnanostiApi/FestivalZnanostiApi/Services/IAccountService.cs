using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Services
{
    public interface IAccountService
    {
        public Task<int> CreateAccount(RegisterDto registerDto);

        public Task<Account?> GetAccountByEmail(string email);

        public Task<AccountDto> GetAccount(int id);

        public Task UserForgotPassword(int accountId, string NewPassword, string ConfirmNewPassword);

        public Task ChangeMyPassword(ChangePasswordDto changePasswordDto);
    }
}
