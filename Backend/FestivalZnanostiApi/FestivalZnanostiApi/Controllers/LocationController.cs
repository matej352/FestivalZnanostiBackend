using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FestivalZnanostiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }





        [HttpGet]
        [Route("ParentLocations")]
        public async Task<IEnumerable<LocationDto>> GetParentLocations()
        {
            var locations = await _locationService.GetParentLocations();

            return locations;
        }


        [HttpGet]
        [Route("ChildLocations")]
        public async Task<IEnumerable<LocationDto>> GetChildLocations(int parentLocationId)
        {
            var locations = await _locationService.GetChildLocations(parentLocationId);

            return locations;
        }



        /*[HttpGet]
        [Route("ChildLocations/Trackable")]
        public IEnumerable<string> GetTrackableChildLocations()
        {
            return new string[] { "value1", "value2" };
        }
        */


        // POST api/<LocationController>
        [HttpPost]
        [Route("Parent/Create")]
        public async Task<LocationDto> CreateParentLocation(string locationName)
        {
            var location = await _locationService.CreateParentLocation(locationName);
            return location;
        }


        // POST api/<LocationController>
        [HttpPost]
        [Route("Child/Create")]
        public async Task<LocationDto> CreateChildLocation(string locationName, int parentId)
        {
            var location = await _locationService.CreateChildLocation(locationName, parentId);
            return location;
        }



        // POST api/<LocationController>
        [HttpPost]
        [Route("Merge")]
        public async Task<ActionResult> MergeLocations(List<int> locationIds, int mergeIntoLocationId)   //  for example locationIds=[1, 5, 3]  mergeIntoLocationId=3
        {
            await _locationService.MergeLocations(locationIds, mergeIntoLocationId);
            return Ok();
        }


        // PUT api/<LocationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LocationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
