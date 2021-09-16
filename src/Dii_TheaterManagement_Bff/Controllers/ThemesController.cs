using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Dii_TheaterManagement_Bff.Clients;
using System.Linq;
using System.Threading.Tasks;

namespace Dii_TheaterManagement_Bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly OrderingSvcClient _orderingSvcClient;

        public ThemesController(OrderingSvcClient orderingSvcClient)
        {
            _orderingSvcClient = orderingSvcClient;
        }

        // GET: api/<ThemesController>
        [HttpGet]
        public async Task<IEnumerable<WebSiteTheme>> Get()
        {
            var themes = await _orderingSvcClient.ApiThemesAsync();
            return themes.ToList();
        }
    }
}
