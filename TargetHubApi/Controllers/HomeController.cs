using System.Web.Http;

namespace TargetHubApi.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Hello World!");
        }
    }
}
