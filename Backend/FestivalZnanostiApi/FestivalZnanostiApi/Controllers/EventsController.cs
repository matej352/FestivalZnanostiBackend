using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Services;
using FestivalZnanostiApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using FestivalZnanostiApi.Middlewares.UserContext;
using FestivalZnanostiApi.Enums;
using NuGet.Protocol.Core.Types;

namespace FestivalZnanostiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {

        private readonly UserContext _userContext;
        private readonly IEventsService _eventsService;

        public EventsController(
            IEventsService eventsService,
            UserContext userContext)
        {
            _eventsService = eventsService;
            _userContext = userContext;
        }

        /// <summary>
        /// Kreiranje eventa od strane submittera ili admina (admin jedini smije stvarati evente tipa Izložba)
        /// </summary>
        /// <param name="createEvent">Podaci eventa koji se stvara</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<Event>> CreateEvent(CreateEventDto createEvent)
        {

            if (createEvent.Type == EventType.Izlozba && _userContext.Role != UserRole.Administrator)
            {
                return Forbid("You don't have permission to create event of type Izložba!");
            }

            var newEvent = await _eventsService.CreateEvent(createEvent);

            return Ok(newEvent);

        }

        //return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);


        //[Authorize]
        [HttpDelete]
        [Route("DeleteEvent/{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {


            if (_userContext.Role != UserRole.Administrator)
            {

                var submitter = await _eventsService.GetEventSubmitter(id);

                if (_userContext.Id != submitter.Id)
                {
                    return Forbid();
                }

            }

            await _eventsService.DeleteEvent(id);

            return NoContent();

        }


        [Authorize]
        [HttpPut("ChangeStatus/{id}")]
        public async Task<ActionResult> ChangeStatus(int id, [FromBody] EventStatus status)
        {
            if (_userContext.Role != UserRole.Administrator)
            {
                return Forbid();
            }
            await _eventsService.ChangeStatus(id, status);
            return NoContent();
        }





        //[Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {

            var events = await _eventsService.GetAllEvents();

            return Ok(events);


        }



        /// <summary>
        /// Dohvat svih evenata nekog submittera (submitter smije vidjeti isključivo svoje evente, admin može vidjeti svačije evente)
        /// </summary>
        /// <param name="submitterId">Identifikator submittera evenata</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetSubmittersEvents")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetSubmittersEvents(int submitterId)
        {
            if (_userContext.Role != UserRole.Administrator && _userContext.Id != submitterId)
            {
                return Forbid();
            }
            var events = await _eventsService.GetSubmittersEvents(submitterId);

            return Ok(events);
        }


        /// <summary>
        /// Dohvat eventa sa predanim id-em (submitter smije vidjeti isključivo svoj event, admin može vidjeti svačiji event)
        /// </summary>
        /// <param name="id">Identifikator eventa</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetEvent")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            if (_userContext.Role != UserRole.Administrator)
            {
                var submitter = await _eventsService.GetEventSubmitter(id);
                if (submitter.Id != _userContext.Id)
                {
                    return Forbid("You can see events that were submitted by you!");
                }
            }

            var eventDto = await _eventsService.GetEvent(id);

            return Ok(eventDto);
        }


        //[Authorize]
        [HttpPut]
        [Route("UpdateEvent/{id}")]
        public async Task<ActionResult<Event>> UpdateEvent(int id, UpdateEventDto updateEventDto)
        {
            if (id != updateEventDto.Id)
            {
                return BadRequest();
            }

            if (_userContext.Role != UserRole.Administrator)
            {
                var eventSubmitter = await _eventsService.GetEventSubmitter(id);
                if (_userContext.Id != eventSubmitter.Id)
                {
                    return Forbid("You can edit events that were submitted by you!");
                }
            }


            var updatedEvent = await _eventsService.UpdateEvent(updateEventDto);

            return Ok(updatedEvent);


        }



        /*
        
        [HttpGet("{id}")]
        [Route("GetEvent")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            if (_context.Event == null)
            {
                return NotFound();
            }
            var @event = await _context.Event.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }
        */

        /*



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        if (_context.Event == null)
        {
            return NotFound();
        }
        var @event = await _context.Event.FindAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        _context.Event.Remove(@event);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    */
    }
}


