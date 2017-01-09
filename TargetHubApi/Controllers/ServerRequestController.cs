using System.Web.Http;
using TargetHubApi.Infrastructure;
using TargetHubApi.Models;

namespace TargetHubApi.Controllers
{
    public class ServerRequestController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

       
        public bool InsertRequest(int serverID, int serverRequestTypeID)
        {
            ServerRequest req = new ServerRequest()
            {
                ServerId = serverID,
                ServerRequestTypeId = serverRequestTypeID
            };
            db.ServerRequests.Add(req);
            db.SaveChanges();
            return true;
        }
    }
}
