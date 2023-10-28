using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using NuGet.Protocol.Core.Types;

namespace FestivalZnanostiApi.Services.impl
{
    public class EventsService : IEventsService
    {

        private readonly IEventsRepository _eventsRepository;

        private readonly ISubmitterService _submitterService;

        public EventsService(IEventsRepository eventsRepository, ISubmitterService submitterService)
        {
            _eventsRepository = eventsRepository;
            _submitterService = submitterService;
        }

        public Task<Event> CreateEvent(CreateEventDto createEvent)
        {

            // 1. provjeri postoji li u bazi submitter sa emailom (jer je email unique)
            // ako postoji, onda dohvati id od tog submittera u bazi i ides dalje
            //              Provjeris i je li u bazi on ima ili nema password, ako nije imao password a sad je dosao createEvent.Password, onda updates submittera u bazi tako da sad ima password
            // inace ides kreirat novog submittera

            var submitterId = _submitterService.CreateSubmitter(createEvent.Submitter).Result;

            var eventId = _eventsRepository.SaveEvent(createEvent, submitterId).Result;

            var _event = _eventsRepository.GetEvent(eventId);

            //int eventId = await _eventsRepository.Save(newKrstarenje);

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
