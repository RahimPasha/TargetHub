using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TargetHubApi.Models
{
    public class ServerRequest
    {
        public int Id { get; set; }
        
        public int ServerId { get; set; }

        public int ServerRequestTypeId { get; set; }

        [ForeignKey("ServerId")]
        public virtual Server Server { get; set; }

        [ForeignKey("ServerRequestTypeId")]
        public virtual ServerRequestType ServerRequestType { get; set; }
    }
}