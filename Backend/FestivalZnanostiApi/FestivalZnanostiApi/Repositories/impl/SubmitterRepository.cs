using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Utils;

namespace FestivalZnanostiApi.Repositories.impl
{
    public class SubmitterRepository : ISubmitterRepository
    {

        private readonly FestivalZnanostiContext _context;

        public SubmitterRepository(FestivalZnanostiContext context)
        {
            _context = context;
        }


        public async Task<int> SaveSubmitter(SubmitterDto submitter)
        {
            string password = null;
            string salt = null;

            if (submitter.Password is not null)
            {
                var passwordHashingHandler = new PasswordHashingHandler(submitter.Password);
                passwordHashingHandler.CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt);

                password = Convert.ToBase64String(passwordHash);
                salt = Convert.ToBase64String(passwordSalt);
            }


            Submitter newSubmitter = new Submitter
            {
                Email = submitter.Email,
                Password = password,
                Salt = salt,
                Role = (int)UserRole.Submitter
            };

            _context.Add(newSubmitter);
            await _context.SaveChangesAsync();

            return await Task.FromResult(newSubmitter.Id);
        }
    }
}
