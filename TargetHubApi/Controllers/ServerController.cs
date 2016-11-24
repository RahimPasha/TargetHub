using System.Linq;
using System.Web.Http;
using TargetHubApi.Infrastructure;
using TargetHubApi.Models;

namespace TargetHubApi.Controllers
{

    public class ServerController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ServerRequestController src = new ServerRequestController();
        [HttpGet]
        public IHttpActionResult Register(string server, string id)
        {
            Server s = new Server()
            {
                Identifier = id,
                Name = server
            };
            db.Servers.Add(s);
            db.SaveChanges();
            src.InsertRequest(s.Id, 1);
            return Ok("Added!");
        }

        [HttpGet]
        public IHttpActionResult GetServers()
        {
            var servers = db.Servers.Select(x => new
            {
                x.Identifier,
                x.Name,
                Requests = x.ServerRequests.Select(r => new
                {
                    r.Id,
                    r.ServerRequestType.Type
                })
            }).ToList();

            return Ok(servers);
        }
    }
}
