using Microsoft.AspNetCore.Mvc;
using Dii_TheaterManagement_Bff.Features.FakeUsers;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dii_TheaterManagement_Bff.Controllers
{
    [Route("fake-users")]
    [ApiController]
    public class FakeUsersController : ControllerBase
    {
        private readonly FakeUserFactory _fakeUserFactory;

        public FakeUsersController(FakeUserFactory fakeUserFactory)
        {
            _fakeUserFactory = fakeUserFactory;
        }

        // GET: api/<FakeUsersController>
        [HttpGet]
        public IEnumerable<FakeUser> Get()
        {
            return _fakeUserFactory.GetAllUsers();
        }

        // GET api/<FakeUsersController>/5
        [HttpGet("{sub}")]
        public FakeUser Get(string sub)
        {
            return _fakeUserFactory.GetUser(sub);
        }
    }
}
