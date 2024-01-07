using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface ITimeSlotRepository
    {

        public Task SaveTimeSlots(List<TimeSlotTemp> timeSlots);



        public Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlots(int locationId, int activeFestivalYearId, bool isIzlozba);


    }
}
