using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FestivalZnanostiApi.Repositories.impl
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly FestivalZnanostiContext _context;

        public TimeSlotRepository(FestivalZnanostiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlots(int locationId, int activeFestivalYearId, bool isIzlozba)
        {

            IEnumerable<TimeSlotDto> availableTimeSlots;

            var trackableLocationsIds = await _context.Location.Where(l => l.TimeSlotsTracked).Select(l => l.Id).ToListAsync();

            // Get timeslots for which we do not track BookedCount and Location
            if (!trackableLocationsIds.Contains(locationId))
            {
                availableTimeSlots = await _context.TimeSlot
                           .Where(ts => ts.LocationId == null &&
                                  ts.FestivalYearId == activeFestivalYearId)
                           .Select(ts => new TimeSlotDto
                           {
                               Id = ts.Id,
                               Start = ts.Start,
                           })
                           .ToListAsync();
            }
            // Get timeslots for which we track BookedCount and Location
            else
            {
                // Only if event type is Izlozba, then we do not trck BookedCount in Tehnicki Muzej
                if (isIzlozba)
                {
                    availableTimeSlots = await _context.TimeSlot
                          .Where(ts => ts.LocationId == locationId &&
                                  ts.FestivalYearId == activeFestivalYearId
                                 )
                          .Select(ts => new TimeSlotDto
                          {
                              Id = ts.Id,
                              Start = ts.Start,
                          })
                          .ToListAsync();
                }
                else
                {
                    availableTimeSlots = await _context.TimeSlot
                           .Where(ts => ts.LocationId == locationId &&
                                   ts.FestivalYearId == activeFestivalYearId &&
                                   ts.BookedCount < ts.Location.ParallelEventCount)
                           .Select(ts => new TimeSlotDto
                           {
                               Id = ts.Id,
                               Start = ts.Start,
                           })
                           .ToListAsync();
                }

            }

            return availableTimeSlots;
        }

        public async Task SaveTimeSlots(List<TimeSlotTemp> timeSlots)
        {

            IEnumerable<TimeSlot> slots = timeSlots.Select(timeSlot => new TimeSlot
            {
                Start = timeSlot.StartTime,
                BookedCount = 0,
                LocationId = timeSlot.LocationId,
                FestivalYearId = timeSlot.FestivalYearId,
            });


            await _context.TimeSlot.AddRangeAsync(slots);
            await _context.SaveChangesAsync();
        }
    }
}
