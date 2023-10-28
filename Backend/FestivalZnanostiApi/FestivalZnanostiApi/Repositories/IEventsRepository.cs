using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface IEventsRepository
    {
        public Task<int> SaveEvent(CreateEventDto createEvent, int submitterId);

        public Task<Event> GetEvent(int id);

        public Task<IEnumerable<Event>> GetEvents();

        public Task<IEnumerable<Event>> GetEvents(int submitterId);
    }
}
