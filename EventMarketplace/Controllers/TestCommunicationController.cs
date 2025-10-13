using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMarketplace.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TestCommunicationController : ControllerBase
    {
        // GET: api/<TestCommunicationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TestCommunicationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestCommunicationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestCommunicationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestCommunicationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
