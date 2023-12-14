using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.Models
{
    public abstract class WarehouseItemEvent
    {
        public double Change { get; set; }
        public double Quantity { get; set; }

        [Key]
        public DateTime Time { get; set; }
    }
}
