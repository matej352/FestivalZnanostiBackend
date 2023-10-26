using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Services
{
    public interface IFilesService
    {

        public byte[] GenerateEventSummary(int EventId);

        public byte[] GenerateFestivalTable(int FestivalYear);
    }
}
