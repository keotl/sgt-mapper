using Microsoft.AspNetCore.Mvc;

namespace app.Controllers {
  [ApiController]
  [Route("ping")]
  public class PingController : ControllerBase {
    public IActionResult Ping() {
      return Ok("Pong!");
    }
  }
}
