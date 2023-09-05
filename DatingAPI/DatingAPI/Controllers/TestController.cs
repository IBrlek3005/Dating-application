using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("getName")]
        public string getName()
        {
            return "Alen";
        }
    }
}
