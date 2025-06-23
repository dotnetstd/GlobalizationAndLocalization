using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace decoupled_poc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocaleController : ControllerBase
    {
        [HttpGet("{language}")]
        [Produces("application/json")]
        public ActionResult Get(string language)
        {
            // Hardcoding for demo purposes. This could be read from a static JSON file, a database or
            // converted from a resx file.
            string json = @"{""about"": {
                ""nav-home"": {
                    ""en-ca"": ""Home"",
                    ""fr-ca"": ""Maison""
                },
                ""nav-about"": {
                    ""en-ca"": ""About"",
                    ""fr-ca"": ""Sur""
                },
                ""bodyCopy"": {
                    ""en-ca"": ""The about page fetches the localized data from an API endpoint."",
                    ""fr-ca"": ""La page about récupère les données localisées à partir d'un point de terminaison d'API.""
                },
                ""langToggle"": {
                    ""en-ca"": ""français"",
                    ""fr-ca"": ""english""
                },
                ""pageHeading"": {
                    ""en-ca"": ""About"",
                    ""fr-ca"": ""Sur""
                }
            }}";

            dynamic data = JsonConvert.DeserializeObject(json);

            // Todo: There's no point in sending both languages if only one is requested. Use Linq or similar to reduce the dataset to
            // just the named locale. For demo purposes ONLY we send both

            if (language.ToLower() == "en-ca")
            {
                return Ok(data);
            }
            else
            {
                // Bilingual = 2 languages therefore this must be fr-CA
                return Ok(data);
            }
        }
    }
}