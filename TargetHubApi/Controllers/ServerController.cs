using System.Linq;
using System.Net;
using System.Net.Http;
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
            if(server == null || id == null)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, "server name or id is null!");
            }
            //Todo: Unregister
            if (!Registered(server, id))
            {
                Server s = new Server()
                {
                    Identifier = id,
                    Name = server
                };
                db.Servers.Add(s);
                db.SaveChanges();
                src.InsertRequest(s.Id, 1);
                return Ok("Identifire:" + s.Id.ToString());
            }
            return Ok("This name has been registered");
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

        private bool Registered(string server, string id)
        {
            return db.Servers.Where(s => s.Identifier == id && s.Name == server).Count() == 0 ? false : true;
        }
    }
}
