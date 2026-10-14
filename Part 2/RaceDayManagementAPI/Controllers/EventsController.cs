using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;

namespace RaceDayManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RDApplicationDbInfo _context;
        public EventsController(RDApplicationDbInfo context)
        {
            _context = context;
        }
        /// <summary>
        /// Retrieves all race events along with their assigned categories.
        /// </summary>
        /// <returns> A list of all scheduled events.</returns>
        /// <response code = "200"> Returns the list of events successfully.</response>
        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.Include(e => e.Category).ToListAsync();
        }
        /// <summary>
        /// Retrieves details for a specific race event by its ID.
        /// </summary>
        /// <param name="id"> The unique ID of the event.</param>
        /// <returns> The requested event object.</returns>
        /// <response code = "200"> Returns the event details.</response>
        /// <response code = "404"> If the event with the given ID was not found.</response>
        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var@event = await _context.Events.Include(e =>e.Category).FirstOrDefaultAsync(e => e.EventId == id);
            if (@event == null) return NotFound();
            return (@event);
        }
        /// <summary>
        /// Creates a new race event in the system.
        /// </summary>
        /// <param name="event"> The event details to create.</param>
        /// <returns> The newly created event.</returns>
        /// <response code = "201" > Event created successfully.</response>
        /// <response code = "400" > If the request payload is invalid.</response>
        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new {id = @event.EventId}, @event);
        }
        /// <summary>
        /// Updates an existing race event by its ID.
        /// </summary>
        /// <param name="id"> The unique ID of the event to update.</param>
        /// <param name="event"> The updated event details.</param>
        /// <returns> No content on success.</returns>
        /// <response code = "204" > Event updated successfully.</response>
        /// <response code = "400" > If the URL ID does not match the event body ID.</response>
        /// <response code = "404" > If the event does not exist.</response>
        //PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event @event)
        {
            if(id != @event.EventId) return BadRequest();
            _context.Entry(@event).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Events.Any(e => e.EventId == id)) return NotFound();
                throw;
            }
            return NoContent();
        }
        /// <summary>
        /// Deletes a specific race event by its ID.
        /// </summary>
        /// <param name="id"> The unique ID of the event to delete.</param>
        /// <returns> No content on success.</returns>
        /// <response code = "204" > Event deleted successfully.</response>
        /// <response code = "404" > If the event was not found.</response>
        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
