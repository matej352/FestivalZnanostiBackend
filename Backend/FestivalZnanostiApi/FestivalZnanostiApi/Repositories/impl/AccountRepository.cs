using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace FestivalZnanostiApi.Repositories.impl
{
    public class AccountRepository : IAccountRepository
    {

        private readonly FestivalZnanostiContext _context;

        public AccountRepository(FestivalZnanostiContext context)
        {
            _context = context;
        }


        public async Task<int> CreateAccount(RegisterDto registerDto)
        {
            string password = null;
            string salt = null;

            if (registerDto.Password is not null)
            {
                var passwordHashingHandler = new PasswordHashingHandler(registerDto.Password);
                passwordHashingHandler.CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt);

                password = Convert.ToBase64String(passwordHash);
                salt = Convert.ToBase64String(passwordSalt);
            }


            Account newAccount = new Account
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = password,
                Salt = salt,
                Role = (int)UserRole.Submitter
            };

            _context.Add(newAccount);
            await _context.SaveChangesAsync();

            return await Task.FromResult(newAccount.Id);
        }

        public async Task<Account?> GetAccount(int id)
        {
            return await _context.Account.FindAsync(id);
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _context.Account.FirstOrDefaultAsync(acc => acc.Email == email);
        }
    }
}
