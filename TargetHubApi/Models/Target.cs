using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TargetHubApi.Models
{
    public class Target
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string XmlFilePath { get; set; }
        public string DatFilePath { get; set; }


        public virtual ICollection<TargetRequest> TargetRequests { get; set; }
    }
}