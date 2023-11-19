using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface IEventsRepository
    {
        public Task<int> SaveEvent(CreateEventDto createEvent, int submitterId);

        public Task<int> UpdateEvent(UpdateEventDto updateEvent);

        public Task<Event> GetEvent(int id);

        public Task<IEnumerable<Event>> GetEvents();

        public Task<IEnumerable<Event>> GetEventsForFestivalYear(int festivalYearId);

        public Task<IEnumerable<Event>> GetEvents(int submitterId);

        public Task<Account> GetEventSubmitter(int eventId);

        public Task DeleteEvent(int eventId);
    }
}
