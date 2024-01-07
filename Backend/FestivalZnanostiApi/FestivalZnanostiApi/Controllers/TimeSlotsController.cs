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
        [Route("GetAvailableTimeSlots")]
        public async Task<ActionResult<IEnumerable<TimeSlotDto>>> GetAvailableTimeSlots(int locationId, bool isIzlozba = false)     //ako se ne šalje, default je false
        {
            if (locationId == 1)
            {
                var response = new Response
                {
                    Message = "Lokacija Tehnički muzej Nikola Tesla se ne može odabrati!"
                };

                return BadRequest(response);
            }

            var timeSlots = await _timeSlotService.GetAvailableTimeSlots(locationId, isIzlozba);
            return Ok(timeSlots);
        }









        /*


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

        */
    }
}
