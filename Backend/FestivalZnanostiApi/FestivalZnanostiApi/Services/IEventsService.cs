using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FestivalZnanostiApi.Services
{
    public interface IEventsService
    {
        public Task<Event> CreateEvent(CreateEventDto createEvent);

        public Task<IEnumerable<EventDto>> GetEvents();

        public Task<IEnumerable<EventDto>> GetSubmittersEvents(int submitterId);
    }
}
