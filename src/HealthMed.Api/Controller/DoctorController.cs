using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Api.Controller
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class DoctorController : BaseController
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}