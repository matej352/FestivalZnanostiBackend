using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Services
{
    public interface IEventsService
    {
        public Task<Event> CreateEvent(CreateEventDto createEvent);
    }
}
