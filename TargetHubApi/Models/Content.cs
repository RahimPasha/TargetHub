using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TargetHubApi.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public virtual ICollection<ContentRequest> ContentRequests { get; set; }
    }
}