using Microsoft.AspNetCore.Mvc;

namespace Homedish.Template.Worker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
