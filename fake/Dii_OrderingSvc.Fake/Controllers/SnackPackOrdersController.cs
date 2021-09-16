using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dii_OrderingSvc.Fake.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dii_OrderingSvc.Fake.Controllers
{
    [Route("api/theaters")]
    [ApiController]
    public class SnackPackOrdersController : ControllerBase
    {
        private readonly OrderingSvcContext _context;

        public SnackPackOrdersController(OrderingSvcContext context)
        {
            _context = context;
        }

        // GET: api/SnackPackOrders
        [HttpGet("{theaterCode}/snack-pack-orders")]
        public async Task<ActionResult<IEnumerable<SnackPackOrder>>> GetSnackPackOrders(string theaterCode)
        {
            return await _context.SnackPackOrders
                .AsNoTracking()
                .Where(spo => spo.DetailedTheater.TheaterCode == theaterCode)
                .ToListAsync();
        }

        // GET: api/SnackPackOrders/5
        [HttpGet("{theaterCode}/snack-pack-orders/{monthId}")]
        public async Task<ActionResult<SnackPackOrder>> GetSnackPackOrder(string theaterCode, long monthId)
        {
            var snackPackOrder = await _context.SnackPackOrders
                .AsNoTracking()
                .Where(spo => spo.TheaterCode == theaterCode)
                .SingleOrDefaultAsync(spo => spo.MonthId == monthId);

            if (snackPackOrder == null)
            {
                return NotFound();
            }

            return snackPackOrder;
        }

        // PUT: api/SnackPackOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{theaterCode}/snack-pack-orders/{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutSnackPackOrder(string theaterCode, int monthId, SnackPackOrder snackPackOrder)
        {
            if (monthId != snackPackOrder.MonthId)
            {
                return BadRequest();
            }
            snackPackOrder.TheaterCode = theaterCode;

            try
            {
                var oldSnackPackOrder = _context.SnackPackOrders.SingleOrDefault(b => b.MonthId == monthId && b.TheaterCode == theaterCode);
                if (oldSnackPackOrder != null)
                {
                    _context.SnackPackOrders.Remove(oldSnackPackOrder);
                }
                _context.SnackPackOrders.Add(snackPackOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SnackPackOrderExists(theaterCode, monthId))
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

        // DELETE: api/SnackPackOrders/5
        [HttpDelete("{theaterCode}/snack-pack-orders/{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteSnackPackOrder(string theaterCode, int monthId)
        {
            var snackPackOrder = await _context.SnackPackOrders
                .Where(spo => spo.TheaterCode == theaterCode)
                .SingleOrDefaultAsync(b => b.MonthId == monthId);
            if (snackPackOrder == null)
            {
                return NotFound();
            }
            snackPackOrder.TheaterCode = theaterCode;

            _context.SnackPackOrders.Remove(snackPackOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SnackPackOrderExists(string theaterCode, int monthId)
        {
            return _context.SnackPackOrders
                .Any(e => e.TheaterCode == theaterCode && e.MonthId == monthId);
        }
    }
}
