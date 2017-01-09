using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TargetHubApi.Infrastructure;
using TargetHubApi.Models;

namespace TargetHubApi.Controllers
{
    public class SubscriptionController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();


        public bool InsertSubscription(int targetID, int serverID)
        {
            Subscription sub = new Subscription()
            {
                TargetID = targetID,
                ServerID = serverID
            };
            db.Subscriptions.Add(sub);
            db.SaveChanges();

            return true;
        }
    }
}
