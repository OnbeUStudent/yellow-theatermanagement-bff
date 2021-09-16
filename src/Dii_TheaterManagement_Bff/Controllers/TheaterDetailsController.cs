using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dii_TheaterManagement_Bff.Clients;
using Dii_TheaterManagement_Bff.Features.SyntheticBehavior;

namespace Dii_TheaterManagement_Bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterDetailsController : ControllerBase
    {
        private readonly OrderingSvcClient _orderingSvcClient;
        private readonly UserInfoAccessor _userInfoAccessor;

        public TheaterDetailsController(OrderingSvcClient orderingSvcClient, UserInfoAccessor userInfoAccessor)
        {
            _orderingSvcClient = orderingSvcClient;
            _userInfoAccessor = userInfoAccessor;
        }

        // GET: api/TheaterDetails/5
        [HttpGet]
        public async Task<ActionResult<DetailedTheater>> GetDetailedTheater()
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            return await _orderingSvcClient.ApiTheatersDetailsGetAsync(theaterCode);
        }

        // PUT: api/TheaterDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutDetailedTheater(DetailedTheater detailedTheater)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            await _orderingSvcClient.ApiTheatersDetailsPutAsync(theaterCode, detailedTheater);
            return NoContent();
        }
    }
}
