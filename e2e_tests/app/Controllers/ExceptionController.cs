using Microsoft.AspNetCore.Mvc;

namespace app.Controllers {
  [ApiController]
  [Route("exception")]
  public class ExceptionController : ControllerBase {
    [HttpGet]
    public IActionResult Get() {
      throw new MyException();
    }
  }
}
