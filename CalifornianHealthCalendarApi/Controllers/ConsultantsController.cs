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

        // GET: api/Consultants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consultant>>> GetConsultants()
        {
            if (_context.Consultants == null)
            {
                return NotFound();
            }

            return await _context.Consultants.ToListAsync();
        }

        // GET: api/Consultants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Consultant>> GetConsultant(int id)
        {
            if (_context.Consultants == null)
            {
                return NotFound();
            }

            var consultant = await _context.Consultants.FindAsync(id);

            if (consultant == null)
            {
                return NotFound();
            }

            return consultant;
        }
        
        // GET: api/ConsultantsAvailabilities/5
        [HttpGet("{consultantId}/ConsultantAvailabilities")]
        public async Task<IEnumerable<ConsultantCalendar>?> GetConsultantAvailabilities(int consultantId)
        {
            if (_context.ConsultantCalendars == null)
            {
                return null;
            }
            
            var consultantCalendar = await _context.ConsultantCalendars.Select(c => c).Where(c => c.ConsultantId == consultantId).ToListAsync();

            return consultantCalendar.Count > 0 ? consultantCalendar : null;
        }
    }
}