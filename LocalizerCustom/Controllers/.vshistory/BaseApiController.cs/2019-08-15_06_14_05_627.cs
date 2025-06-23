using Microsoft.AspNetCore.Mvc;

namespace LocalizerCustom.Controllers
{
    [Route("api/{language:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/[controller]")]
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
    }
    public class HelloWorldController : BaseApiController
    {
        [HttpGet]
        public string SayHello()
        {
            return "Hello World";
        }
    }
}
