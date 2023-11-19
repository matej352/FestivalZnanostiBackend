using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Middlewares.UserContext;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using FestivalZnanostiApi.Servicess;
using NuGet.Protocol.Core.Types;

namespace FestivalZnanostiApi.Services.impl
{
    public class EventsService : IEventsService
    {

        private readonly UserContext _userContext;

        private readonly IEventsRepository _eventsRepository;

        private readonly IAccountService _submitterService;

        private readonly IFestivalYearService _festivalYearService;

        public EventsService(IEventsRepository eventsRepository, IAccountService submitterService, UserContext userContext, IFestivalYearService festivalYearService)
        {
            _eventsRepository = eventsRepository;
            _submitterService = submitterService;
            _userContext = userContext;
            _festivalYearService = festivalYearService;
        }

        public async Task<Event> CreateEvent(CreateEventDto createEvent)
        {
            var eventId = await _eventsRepository.SaveEvent(createEvent, _userContext.Id);

            var _event = await _eventsRepository.GetEvent(eventId);

            return _event;



        }


        public async Task<Event> UpdateEvent(UpdateEventDto updateEvent)
        {
            var eventId = await _eventsRepository.UpdateEvent(updateEvent);

            var _event = await _eventsRepository.GetEvent(eventId);

            return _event;
        }


        public async Task<IEnumerable<EventDto>> GetEvents(int? festivalYearId)
        {

            if (festivalYearId == null)
            {
                festivalYearId = (await _festivalYearService.GetActiveFestivalYear()).Id;
            }
            var events = await _eventsRepository.GetEventsForFestivalYear((int)festivalYearId);


            List<EventDto> eventDtoList = events.Select(e => e.AsEventDto()).ToList();

            return eventDtoList;
        }

        public async Task<IEnumerable<PdfEventDto>> GetPdfEvents(int? festivalYearId)
        {
            if (festivalYearId == null)
            {
                festivalYearId = (await _festivalYearService.GetActiveFestivalYear()).Id;
            }
            var events = await _eventsRepository.GetEventsForFestivalYear((int)festivalYearId);

            List<PdfEventDto> pdfEventDtoList = events.Select(e => e.AsPdfEventDto()).ToList();

            return pdfEventDtoList;
        }


        public async Task<IEnumerable<EventDto>> GetAllEvents()
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

        public async Task<AccountDto> GetEventSubmitter(int eventId)
        {
            var submitter = await _eventsRepository.GetEventSubmitter(eventId);
            return submitter.AsAccountDto();
        }


    }
}
