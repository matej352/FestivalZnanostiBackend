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

        public async Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlots(int locationId, DateTime start, DateTime end)
        {
            var availableTimeSlots = await _context.TimeSlot
             .Where(ts => ts.LocationId == locationId &&
                          ts.Start >= start && ts.Start <= end &&
                          ts.BookedCount < ts.Location.ParallelEventCount)
             .Select(ts => new TimeSlotDto
             {
                 Id = ts.Id,
                 Start = ts.Start,
             })
             .ToListAsync();

            return availableTimeSlots;
        }

        public async Task SaveTimeSlots(List<TimeSlotTemp> timeSlots)
        {

            IEnumerable<TimeSlot> slots = timeSlots.Select(timeSlot => new TimeSlot
            {
                Start = timeSlot.StartTime,
                BookedCount = 0,
                LocationId = timeSlot.LocationId
            });


            await _context.TimeSlot.AddRangeAsync(slots);
            await _context.SaveChangesAsync();
        }
    }
}
