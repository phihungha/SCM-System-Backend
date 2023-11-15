
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public abstract class OrderEvent
    {
        public int Id { get; set; }

        public OrderEventType Type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Time { get; set; }

        public string Location { get; set; }
        public string? Message { get; set; }
    }
}
