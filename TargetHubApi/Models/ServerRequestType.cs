using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TargetHubApi.Models
{
    public class ServerRequestType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ServerRequest> ServerRequests { get; set; }

    }
}