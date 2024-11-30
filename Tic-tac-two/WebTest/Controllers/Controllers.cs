using Microsoft.AspNetCore.Mvc;

namespace WebTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NamesController : ControllerBase
    {
        [HttpGet]
        public JsonResult GetNames()
        {
            var names = new string[3] { "John", "Clara", "Valeri" };
            return new JsonResult(names);
        }
        
        [HttpPost]
        public IActionResult PostNames([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name cannot be empty.");
            }
            return Ok(new { message = "Name received successfully", name });
        }
    }
}