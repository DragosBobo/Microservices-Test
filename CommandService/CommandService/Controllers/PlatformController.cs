
using Microsoft.AspNetCore.Mvc;
namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformController : ControllerBase
    {

        public PlatformController(  )
        {

        }
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound Post #Comand SErvice");
            return Ok("Inbound test ok from Platfroms controller");
        }


    }

}
