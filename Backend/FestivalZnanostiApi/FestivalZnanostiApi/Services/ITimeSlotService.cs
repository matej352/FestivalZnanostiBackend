using FestivalZnanostiApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FestivalZnanostiApi.Services
{
    public interface ITimeSlotService
    {

        public Task CreateTimeSlots(DateTime startDate, DateTime endDate, int festivalYearId);


        public Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlots(int locationId, bool isIzlozba);


    }
}
