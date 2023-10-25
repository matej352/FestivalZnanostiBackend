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

namespace FestivalZnanostiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }


        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(CreateEventDto createEvent)
        {

            var newEvent = await _eventsService.CreateEvent(createEvent);

            return Ok(newEvent);


        }

        //return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
    }


    /*
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvent()
    {
      if (_context.Event == null)
      {
          return NotFound();
      }
        return await _context.Event.ToListAsync();
    }


    [HttpGet("{id}")]
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

    private bool EventExists(int id)
    {
        return (_context.Event?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    */
}


