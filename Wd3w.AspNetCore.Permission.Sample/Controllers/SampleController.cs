using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Wd3w.AspNetCore.Permission.Sample.Controllers
{
    [ApiController]
    [Route("api/sample")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;

        public SampleController(ILogger<SampleController> logger)
        {
            _logger = logger;
        }

        [HttpGet("basic-permission-api")]
        [Permission("ReadableSamplePermission")]
        public ActionResult GetSampleAsync()
        {
            return Ok("Hello there!");
        }
    }
}