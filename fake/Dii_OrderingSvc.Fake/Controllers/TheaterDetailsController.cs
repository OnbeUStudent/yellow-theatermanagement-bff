using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dii_OrderingSvc.Fake.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dii_OrderingSvc.Fake.Controllers
{
    [Route("api/theaters")]
    [ApiController]
    public class TheaterDetailsController : ControllerBase
    {
        private readonly OrderingSvcContext _context;

        public TheaterDetailsController(OrderingSvcContext context)
        {
            _context = context;
        }

        // GET: api/TheaterDetails/5
        [HttpGet("{theaterCode}/details")]
        public async Task<ActionResult<DetailedTheater>> GetDetailedTheater(string theaterCode)
        {
            var detailedTheater = await _context.DetailedTheaters
                .SingleAsync(t => t.TheaterCode == theaterCode);
            if (detailedTheater == null)
            {
                return NotFound();
            }

            return detailedTheater;
        }

        // PUT: api/TheaterDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{theaterCode}/details")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutDetailedTheater(string theaterCode, DetailedTheater detailedTheater)
        {
            detailedTheater.TheaterCode = theaterCode;

            _context.Entry(detailedTheater).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailedTheaterExists(theaterCode))
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

        private bool DetailedTheaterExists(string theaterCode)
        {
            return _context.DetailedTheaters.Any(e => e.TheaterCode == theaterCode);
        }
    }
}
