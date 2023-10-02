using CalifornianHealthLib.Context;
using CalifornianHealthLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalifornianHealthCalendarApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantsController : ControllerBase
    {
        private readonly ChDbContext _context;

        public ConsultantsController(ChDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a list of all consultants.
        /// </summary>
        /// <returns>A list of consultants.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consultant>>> GetConsultants()
        {
            if (_context.Consultants == null)
            {
                return NotFound();
            }

            return await _context.Consultants.ToListAsync();
        }

        /// <summary>
        /// Get a consultant by their ID.
        /// </summary>
        /// <param name="id">The ID of the consultant.</param>
        /// <returns>The consultant with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Consultant>> GetConsultant(int id)
        {
            if (_context.Consultants == null || await _context.Consultants.FindAsync(id) == null)
            {
                return NotFound();
            }
            return consultant;
        }

        /// <summary>
        /// Get a list of consultant availabilities by consultant ID.
        /// </summary>
        /// <param name="consultantId">The ID of the consultant.</param>
        /// <returns>A list of consultant availabilities.</returns>
        [HttpGet("{consultantId}/ConsultantAvailabilities")]
        public async Task<IEnumerable<ConsultantCalendar>?> GetConsultantAvailabilities(int consultantId)
        {
            if (_context.ConsultantCalendars == null)
            {
                return null;
            }

            var consultantCalendar = await _context.ConsultantCalendars
                .Where(c => c.ConsultantId == consultantId)
                .ToListAsync();

            return consultantCalendar.Count > 0 ? consultantCalendar : null;
        }
    }
}
