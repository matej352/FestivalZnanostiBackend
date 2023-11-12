using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Services
{
    public interface IFilesService
    {

        public Task<byte[]> GenerateEventsSummary();

        public byte[] GenerateFestivalTable(int FestivalYear);
    }
}
