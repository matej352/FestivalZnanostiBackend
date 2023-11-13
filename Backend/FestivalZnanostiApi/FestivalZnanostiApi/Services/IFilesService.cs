using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Services
{
    public interface IFilesService
    {

        public Task<byte[]> GenerateEventsSummary(int? festivalYearId = null);

        public Task<byte[]> GenerateFestivalTable(int? festivalYearId = null);
    }
}
