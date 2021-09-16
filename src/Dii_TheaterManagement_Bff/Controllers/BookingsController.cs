using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dii_TheaterManagement_Bff.Clients;
using Dii_TheaterManagement_Bff.Features.SyntheticBehavior;

namespace Dii_TheaterManagement_Bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly OrderingSvcClient _orderingSvcClient;
        private readonly UserInfoAccessor _userInfoAccessor;

        public BookingsController(OrderingSvcClient orderingSvcClient, UserInfoAccessor userInfoAccessor)
        {
            _orderingSvcClient = orderingSvcClient;
            _userInfoAccessor = userInfoAccessor;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            var bookings = await _orderingSvcClient.ApiTheatersBookingsGetAsync(theaterCode);
            return bookings.ToList();
        }

        // GET: api/Bookings/5
        [HttpGet("{monthId}")]
        public async Task<ActionResult<Booking>> GetBooking(int monthId)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            return await _orderingSvcClient.ApiTheatersBookingsGetAsync(theaterCode, monthId);
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutBooking(int monthId, Booking booking)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            await _orderingSvcClient.ApiTheatersBookingsPutAsync(theaterCode, monthId, booking);
            return NoContent();
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteBooking(int monthId)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            await _orderingSvcClient.ApiTheatersBookingsDeleteAsync(theaterCode, monthId);
            return NoContent();
        }
    }
}
