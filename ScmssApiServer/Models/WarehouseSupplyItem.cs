using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class WarehouseSupplyItem : WarehouseItem
    {
        public int SupplyId { get; set; }
        public Supply Supply { get; set; } = null!;

        [NotMapped]
        public override string Unit => Supply.Unit;

        [NotMapped]
        public override decimal UnitValue => Supply.Price;
    }
}
