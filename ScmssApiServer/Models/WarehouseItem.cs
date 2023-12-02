using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public abstract class WarehouseItem : ISoftDeletable
    {
        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;

        public double Quantity { get; set; }

        [NotMapped]
        public abstract string Unit { get; }

        [NotMapped]
        public abstract decimal UnitValue { get; }

        [NotMapped]
        public decimal TotalValue => UnitValue * (decimal)Quantity;

        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
