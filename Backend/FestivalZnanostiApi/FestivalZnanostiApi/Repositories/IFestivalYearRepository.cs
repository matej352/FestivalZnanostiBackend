using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface IFestivalYearRepository
    {
        public Task<IEnumerable<FestivalYearDto>> GetFestivalYears();

        public Task<FestivalYearDto> GetFestivalYear(int id);

        public Task<int> CreateFestivalYear(CreateFestivalYearDto festivalYear);

        public Task<FestivalYear> FindById(int id);

        public Task<FestivalYearDto> FindActiveFestivalYear();

        public Task<int> UpdateFestivalYear(UpdateFestivalYearDto updateFestivalYear);

        public Task ChangeFestivalYearActiveStatus(int id, FestivalYearActivityStatus active);

    }
}
