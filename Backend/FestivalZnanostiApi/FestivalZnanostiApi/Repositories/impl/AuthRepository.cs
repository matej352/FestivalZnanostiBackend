using FestivalZnanostiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FestivalZnanostiApi.Repositories.impl
{
    public class AuthRepository : IAuthRepository
    {


        private readonly FestivalZnanostiContext _context;


        public AuthRepository(FestivalZnanostiContext context)
        {
            _context = context;
        }


        public async Task<int?> FindByEmail(string email)
        {
            var account = await _context.Account.Where(acc => acc.Email == email).SingleOrDefaultAsync();

            if (account == null)
            {
                return null;
            }
            else
            {
                return account.Id;
            }
        }
    }
}
