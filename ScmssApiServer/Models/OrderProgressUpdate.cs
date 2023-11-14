
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public abstract class OrderProgressUpdate
    {
        public int Id { get; set; }

        public OrderProgressUpdateType Type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Time { get; set; }

        public required string Location { get; set; }
        public string? Message { get; set; }
    }
}
