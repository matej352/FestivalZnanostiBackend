using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FestivalZnanostiApi.Services
{
    public interface IEventsService
    {
        public Task<Event> CreateEvent(CreateEventDto createEvent);

        public Task<IEnumerable<EventDto>> GetEvents(int? festivalYearId);

        public Task<IEnumerable<PdfEventDto>> GetPdfEvents(int? festivalYearId);

        public Task<IEnumerable<EventDto>> GetAllEvents();

        public Task<IEnumerable<EventDto>> GetSubmittersEvents(int submitterId);

        public Task<EventDto> GetEvent(int id);

        public Task<AccountDto> GetEventSubmitter(int eventId);

        public Task<EventDto> UpdateEvent(UpdateEventDto updateEvent);

        public Task DeleteEvent(int eventId);

        public Task ChangeStatus(int eventId, EventStatus status);
    }
}
