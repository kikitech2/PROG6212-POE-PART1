using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;

namespace RaceDayManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly RDApplicationDbInfo _context;

        public RoutesController(RDApplicationDbInfo context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all race routes available in the system.
        /// </summary>
        /// <returns>A list of all routes.</returns>
        /// <response code="200">Returns the list of routes successfully.</response>
        // GET: api/Routes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteInfo>>> GetRoutes()
        {
            return await _context.Routes.ToListAsync();
        }

        /// <summary>
        /// Retrieves details for a specific route by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the route.</param>
        /// <returns>The requested route object.</returns>
        /// <response code="200">Returns the route details.</response>
        /// <response code="404">If the route with the given ID was not found.</response>
        // GET: api/Routes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RouteInfo>> GetRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return NotFound();
            return route;
        }

        /// <summary>
        /// Creates a new race route in the system.
        /// </summary>
        /// <param name="route">The route details to create.</param>
        /// <returns>The newly created route.</returns>
        /// <response code="201">Route created successfully.</response>
        /// <response code="400">If the request payload is invalid.</response>
        // POST: api/Routes
        [HttpPost]
        public async Task<ActionResult<RouteInfo>> CreateRoute(RouteInfo route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoute), new { id = route.RouteId }, route);
        }

        /// <summary>
        /// Updates an existing route by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the route to update.</param>
        /// <param name="route">The updated route details.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Route updated successfully.</response>
        /// <response code="400">If the URL ID does not match the route payload ID.</response>
        /// <response code="404">If the route does not exist.</response>
        // PUT: api/Route/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(int id, RouteInfo route)
        {
            if (id != route.RouteId) return BadRequest();
            _context.Entry(route).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Routes.Any(r => r.RouteId == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific route by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the route to delete.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Route deleted successfully.</response>
        /// <response code="404">If the route was not found.</response>
        // DELETE: api/Routes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return NotFound();
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
