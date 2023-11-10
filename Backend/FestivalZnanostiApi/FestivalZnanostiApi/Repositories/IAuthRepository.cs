using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface IAuthRepository
    {



        public Task<int?> FindByEmail(string email);

    }
}
