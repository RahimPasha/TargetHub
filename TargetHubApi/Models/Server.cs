using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TargetHubApi.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual ICollection<ServerRequest> ServerRequests { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

    }
}