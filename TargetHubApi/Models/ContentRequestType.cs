using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TargetHubApi.Models
{
    public class ContentRequestType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ContentRequest> ContentRequests { get; set; }

    }
}