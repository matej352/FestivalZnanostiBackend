using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface IFestivalYearRepository
    {
        public Task<IEnumerable<FestivalYear>> GetFestivalYear();

        public Task<int> CreateFestivalYear(FestivalYear festivalYear);

        public Task<FestivalYear> FindById(int id);
    }
}
