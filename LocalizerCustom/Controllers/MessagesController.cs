using LocalizerCustom.Modelss;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

namespace LocalizerCustom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ISharedResource _sharedResource;

        public MessagesController(ISharedResource sharedResource)
        {
            _sharedResource = sharedResource;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(_sharedResource.MessageOne);
        }
    }
}
