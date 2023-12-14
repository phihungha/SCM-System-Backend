using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public abstract class WarehouseItem : ICreateUpdateTime
    {
        public DateTime CreateTime { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public double Quantity { get; set; }

        [NotMapped]
        public decimal TotalValue => UnitValue * (decimal)Quantity;

        [NotMapped]
        public abstract string Unit { get; }

        [NotMapped]
        public abstract decimal UnitValue { get; }

        public DateTime? UpdateTime { get; set; }
    }
}
