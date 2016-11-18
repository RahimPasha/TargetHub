using System;
using System.Collections.Generic;
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
            return Ok("Added!");
        }


    }
}
