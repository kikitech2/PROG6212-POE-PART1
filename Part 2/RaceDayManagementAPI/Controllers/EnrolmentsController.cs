using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;

namespace RaceDayManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RDApplicationDbInfo _context;

        public EnrolmentsController(RDApplicationDbInfo context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all the event enrolments in the system.
        /// </summary>
        /// <returns>A list of all enrolments with participant, event, and category details.</returns>
        /// <response code="200">Returns the list of enrolments successfully.</response>
        // GET: api/Enrolments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments()
        {
            return await _context.Enrolments
                .Include(e => e.Participant)
                .Include(e => e.Event)
                .ToListAsync();
        }

        /// <summary>
        /// Registers a user for a race event under a specific category.
        /// </summary>
        /// <param name="enrolment">The enrolment details to create.</param>
        /// <returns>The newly created enrolment record.</returns>
        /// <response code="201">Enrolment created successfully.</response>
        /// <response code="400">If the enrolment payload is invalid.</response>
        // POST: api/Enrolments - Register a user for an event
        [HttpPost]
        public async Task<ActionResult<Enrolment>> PostEnrolment(Enrolment enrolment)
        {
            enrolment.Participant = null;
            enrolment.Event = null;
            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEnrolments), new { id = enrolment.EnrolmentId }, enrolment);
        }

        /// <summary>
        /// Cancels a participant's event enrolment by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the enrolment to delete.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Enrolment deleted successfully.</response>
        /// <response code="404">If the enrolment was not found.</response>
        // DELETE: api/Enrolments/5 (Cancel registration)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);
            if (enrolment == null) return NotFound();
            _context.Enrolments.Remove(enrolment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
