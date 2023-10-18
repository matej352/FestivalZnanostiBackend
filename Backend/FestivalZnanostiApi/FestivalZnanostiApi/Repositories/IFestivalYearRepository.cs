using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface IFestivalYearRepository
    {
        public Task<IEnumerable<FestivalYear>> GetFestivalYear();
    }
}
