using Microsoft.AspNetCore.Mvc;

namespace nginx_deployer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeployController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // TODO: Implement your logic here
            return Ok("Hello, World!");
        }

        [HttpPost]
        public IActionResult Post([FromBody] YourModel model)
        {
            // TODO: Implement your logic here
            return Ok("Data received successfully!");
        }
    }

    public class YourModel
    {
        // TODO: Add your model properties here
    }
}