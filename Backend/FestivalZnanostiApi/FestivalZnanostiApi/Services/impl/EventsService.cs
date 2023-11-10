using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Middlewares.UserContext;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using NuGet.Protocol.Core.Types;

namespace FestivalZnanostiApi.Services.impl
{
    public class EventsService : IEventsService
    {

        private readonly UserContext _userContext;

        private readonly IEventsRepository _eventsRepository;

        private readonly IAccountService _submitterService;

        public EventsService(IEventsRepository eventsRepository, IAccountService submitterService, UserContext userContext)
        {
            _eventsRepository = eventsRepository;
            _submitterService = submitterService;
            _userContext = userContext;
        }

        public Task<Event> CreateEvent(CreateEventDto createEvent)
        {

            //var submitterId = _submitterService.CreateSubmitter(createEvent.Submitter).Result; TO JE ZAPRAVO REGISTRATION


            var eventId = _eventsRepository.SaveEvent(createEvent, _userContext.Id).Result;

            var _event = _eventsRepository.GetEvent(eventId);

            return _event;



        }

        public async Task<IEnumerable<EventDto>> GetEvents()
        {
            var events = await _eventsRepository.GetEvents();

            List<EventDto> eventDtoList = events.Select(e => e.AsEventDto()).ToList();

            return eventDtoList;
        }

        public async Task<IEnumerable<EventDto>> GetSubmittersEvents(int id)
        {
            var events = await _eventsRepository.GetEvents(id);

            List<EventDto> eventDtoList = events.Select(e => e.AsEventDto()).ToList();

            return eventDtoList;
        }
    }
}
