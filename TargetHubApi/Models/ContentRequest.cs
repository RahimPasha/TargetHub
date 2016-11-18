using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TargetHubApi.Models
{
    public class ContentRequest
    {
        public int Id { get; set; }

        public int ContentId { get; set; }

        public int ContentRequestTypeId { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        [ForeignKey("ContentRequestTypeId")]
        public virtual ContentRequestType ContentRequestType { get; set; }
    }
}