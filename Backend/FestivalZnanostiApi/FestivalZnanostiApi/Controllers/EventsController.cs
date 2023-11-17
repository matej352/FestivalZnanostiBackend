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



        //[Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {

            var events = await _eventsService.GetAllEvents();

            return Ok(events);


        }


        [Authorize(Roles = "Submitter")]
        [HttpGet]
        [Route("GetSubmittersEvents")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetSubmittersEvents(int submitterId)
        {
            if (_userContext.Id != submitterId)
            {
                return Forbid();
            }
            var events = await _eventsService.GetSubmittersEvents(submitterId);

            return Ok(events);
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
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvent(int id, Event @event)
    {
        if (id != @event.Id)
        {
            return BadRequest();
        }

        _context.Entry(@event).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EventExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


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


