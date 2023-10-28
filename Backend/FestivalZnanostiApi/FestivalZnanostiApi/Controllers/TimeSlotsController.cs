using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FestivalZnanostiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeSlotsController : ControllerBase
    {

        private ITimeSlotService _timeSlotService;

        public TimeSlotsController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }




        // GET: api/<TimeSlotsController>
        // Dohvaćanje slobodnih termina na lokaciji sa id = locationId za aktivnu godinu (FestivalYear.Active = 1)
        [HttpGet]
        public Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlots(int locationId)
        {
            var timeSlots = _timeSlotService.GetAvailableTimeSlots(locationId);
            return timeSlots;
        }












        // GET api/<TimeSlotsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TimeSlotsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TimeSlotsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TimeSlotsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
