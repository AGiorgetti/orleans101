using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly IClusterClient _client;

        public HelloController(IClusterClient client)
        {
            _client = client;
        }

        // GET api/values/5
        [HttpGet()]
        public async Task<ActionResult<string>> SayAsync(string id, string message)
        {
            // example of calling grains from the initialized client
            var friend = _client.GetGrain<IHello>("Hello1");
            var response = await friend.SayHello(message);
            // Console.WriteLine("\n\n{0}\n\n", response);
            return response;
        }
    }
}