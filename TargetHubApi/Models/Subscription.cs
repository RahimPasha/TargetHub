using System.ComponentModel.DataAnnotations.Schema;

namespace TargetHubApi.Models
{
    public class Subscription
    {
        public int ID { get; set; }
        public int TargetID { get; set; }
        public int ServerID { get; set; }

        [ForeignKey("TargetID")]
        public virtual Target Target { get; set; }

        [ForeignKey("ServerID")]
        public virtual Server Server { get; set; }
    }
}