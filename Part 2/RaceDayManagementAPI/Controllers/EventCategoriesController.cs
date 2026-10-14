using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;

namespace RaceDayManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventCategoriesController : ControllerBase
    {
        private readonly RDApplicationDbInfo _context;

        public EventCategoriesController(RDApplicationDbInfo context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all event categories (e.g., Under 20, Senior, 10km, 21km).
        /// </summary>
        /// <returns>A list of all available event categories.</returns>
        /// <response code="200">Returns the list of categories successfully.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventCategory>>> GetEventCategories()
        {
            return await _context.EventsCategories.ToListAsync();
        }

        /// <summary>
        /// Retrieves details for a specific event category by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the event category.</param>
        /// <returns>The requested event category object.</returns>
        /// <response code="200">Returns the category details.</response>
        /// <response code="404">If the category with the given ID was not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventCategory>> GetEventCategory(int id)
        {
            var category = await _context.EventsCategories.FindAsync(id);
            if (category == null) return NotFound();
            return category;
        }

        /// <summary>
        /// Creates a new event category in the system.
        /// </summary>
        /// <param name="category">The details of the event category to create.</param>
        /// <returns>The newly created event category.</returns>
        /// <response code="201">Event category created successfully.</response>
        /// <response code="400">If the request payload is invalid.</response>
        [HttpPost]
        public async Task<ActionResult<EventCategory>> CreateEventCategory(EventCategory category)
        {
            _context.EventsCategories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEventCategory), new { id = category.CategoryId }, category);
        }

        /// <summary>
        /// Updates an existing event category by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the event category to update.</param>
        /// <param name="category">The updated event category details.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Event category updated successfully.</response>
        /// <response code="400">If the URL ID does not match the category payload ID.</response>
        /// <response code="404">If the category does not exist.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventCategory(int id, EventCategory category)
        {
            if (id != category.CategoryId) return BadRequest();
            _context.Entry(category).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EventsCategories.Any(c => c.CategoryId == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific event category by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the event category to delete.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Event category deleted successfully.</response>
        /// <response code="404">If the event category was not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventCategory(int id)
        {
            var category = await _context.EventsCategories.FindAsync(id);
            if (category == null) return NotFound();
            _context.EventsCategories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
