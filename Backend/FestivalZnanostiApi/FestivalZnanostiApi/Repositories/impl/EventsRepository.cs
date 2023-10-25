using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace FestivalZnanostiApi.Repositories.impl
{
    public class EventsRepository : IEventsRepository
    {


        private readonly FestivalZnanostiContext _context;


        public EventsRepository(FestivalZnanostiContext context)
        {
            _context = context;
        }

        public async Task<Event> GetEvent(int id)
        {
            var _event = await _context.Event.FindAsync(id);
            if (_event is null)
            {
                throw new Exception($"Event with id = {id} does not exists");
            }
            else
            {
                return _event;
            }
        }

        public async Task<int> SaveEvent(CreateEventDto createEvent, int submitterId)
        {
            Event newEvent = new Event
            {
                Title = createEvent.Title,
                Status = (int)EventStatus.Pending,
                Type = mapEventType(createEvent.Type),
                VisitorsCount = createEvent.VisitorsCount,
                Equipment = createEvent.Equipment,
                Summary = createEvent.Summary,
                LocationId = createEvent.LocationId,
                FestivalYearId = createEvent.FestivalYearId,
                SubmitterId = submitterId,

            };

            _context.Add(newEvent);
            await _context.SaveChangesAsync();

            return await Task.FromResult(newEvent.Id);
        }



        private string mapEventType(EventType type)
        {
            switch ((int)type)
            {
                case 0:
                    return "Predavanje";
                case 1:
                    return "Prezentacija";
                case 2:
                    return "Radionica";
                default:
                    throw new Exception("Invalid event type");
            }
        }




    }
}
