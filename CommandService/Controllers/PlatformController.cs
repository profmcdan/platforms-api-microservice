using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/v1/command/platform")]
    public class PlatformController : ControllerBase
    {
        public PlatformController()
        {
            
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            return Ok("Inbound test from the Platform Controllers");
        }
        
    }
}