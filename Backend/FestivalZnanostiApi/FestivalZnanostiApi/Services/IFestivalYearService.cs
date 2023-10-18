using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Servicess
{
    public interface IFestivalYearService
    {
        public Task<IEnumerable<FestivalYear>> Get();
    }
}
