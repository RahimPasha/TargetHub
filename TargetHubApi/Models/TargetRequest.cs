using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TargetHubApi.Models
{
    public class TargetRequest
    {
        public int Id { get; set; }

        public int TargetId { get; set; }

        public int TargetRequestTypeId { get; set; }

        [ForeignKey("TargetId")]
        public virtual Target Target { get; set; }

        [ForeignKey("TargetRequestTypeId")]
        public virtual TargetRequestType TargetRequestTyep { get; set; }

    }
}