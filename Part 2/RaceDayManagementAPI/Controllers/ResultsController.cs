using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;

namespace RaceDayManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RDApplicationDbInfo _context;

        public ResultsController(RDApplicationDbInfo context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all race finish results with participant and event details.
        /// </summary>
        /// <returns>A list of all recorded race results.</returns>
        /// <response code="200">Returns the list of results successfully.</response>
        // GET: api/Results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Result>>> GetResults()
        {
            return await _context.Results
                .Include(r => r.Enrolment!)
                .ThenInclude(e => e.Participant!)
                .Include(r => r.Enrolment!)
                .ThenInclude(e => e.Event!)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves details for a specific race result by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the race result.</param>
        /// <returns>The requested result object.</returns>
        /// <response code="200">Returns the result details.</response>
        /// <response code="404">If the result was not found.</response>
        // GET: api/Results/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetResult(int id)
        {
            var result = await _context.Results
                .Include(r => r.Enrolment!)
                .ThenInclude(e => e.Participant!)
                .Include(r => r.Enrolment!)
                .ThenInclude(e => e.Event!)
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (result == null) return NotFound();
            return result;
        }

        /// <summary>
        /// Records a participant's finish time and position for an event.
        /// </summary>
        /// <param name="result">The result payload to capture.</param>
        /// <returns>The newly recorded result record.</returns>
        /// <response code="201">Result captured successfully.</response>
        /// <response code="400">If the result payload is invalid.</response>
        // POST: api/Results
        [HttpPost]
        public async Task<ActionResult<Result>> PostResult(Result result)
        {
            _context.Results.Add(result);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetResult), new { id = result.ResultId }, result);
        }

        /// <summary>
        /// Updates an existing race result by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the result to update.</param>
        /// <param name="result">The updated result details.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Result updated successfully.</response>
        /// <response code="400">If the URL ID does not match the payload ID.</response>
        /// <response code="404">If the result does not exist.</response>
        // PUT: api/Results/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResult(int id, Result result)
        {
            if (id != result.ResultId) return BadRequest();
            _context.Entry(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Results.Any(r => r.ResultId == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific race result by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the result to delete.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Result deleted successfully.</response>
        /// <response code="404">If the result was not found.</response>
        // DELETE: api/Results/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result == null) return NotFound();
            _context.Results.Remove(result);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
