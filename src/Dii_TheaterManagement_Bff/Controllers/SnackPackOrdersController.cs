using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dii_TheaterManagement_Bff.Clients;
using Dii_TheaterManagement_Bff.Features.SyntheticBehavior;

namespace Dii_TheaterManagement_Bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnackPackOrdersController : ControllerBase
    {
        private readonly OrderingSvcClient _orderingSvcClient;
        private readonly UserInfoAccessor _userInfoAccessor;

        public SnackPackOrdersController(OrderingSvcClient orderingSvcClient, UserInfoAccessor userInfoAccessor)
        {
            _orderingSvcClient = orderingSvcClient;
            _userInfoAccessor = userInfoAccessor;
        }

        // GET: api/SnackPackOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SnackPackOrder>>> GetSnackPackOrders()
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            var spos = await _orderingSvcClient.ApiTheatersSnackPackOrdersGetAsync(theaterCode);
            return spos.ToList();
        }

        // GET: api/SnackPackOrders/5
        [HttpGet("{monthId}")]
        public async Task<ActionResult<SnackPackOrder>> GetSnackPackOrder(long monthId)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            return await _orderingSvcClient.ApiTheatersSnackPackOrdersGetAsync(theaterCode, monthId);
        }

        // PUT: api/SnackPackOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutSnackPackOrder(int monthId, SnackPackOrder snackPackOrder)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            await _orderingSvcClient.ApiTheatersSnackPackOrdersPutAsync(theaterCode, monthId, snackPackOrder);
            return NoContent();
        }

        // DELETE: api/SnackPackOrders/5
        [HttpDelete("{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteSnackPackOrder(int monthId)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            await _orderingSvcClient.ApiTheatersSnackPackOrdersDeleteAsync(theaterCode, monthId);
            return NoContent();
        }
    }
}
