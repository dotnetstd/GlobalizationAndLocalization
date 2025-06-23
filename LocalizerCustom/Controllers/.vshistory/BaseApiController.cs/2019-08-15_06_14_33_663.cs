using Microsoft.AspNetCore.Mvc;

namespace LocalizerCustom.Controllers
{
    [Route("api/{language:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/[controller]")]
    [Route("api/[controller]")]
    ///api/helloworld
    ///api/en/helloworld
    ///api/es-ES/helloworld
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
