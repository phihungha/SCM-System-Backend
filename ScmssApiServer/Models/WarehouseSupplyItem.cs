using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class WarehouseSupplyItem : WarehouseItem
    {
        public int SupplyId { get; set; }
        public Supply Supply { get; set; } = null!;


        public ICollection<WarehouseSupplyItemEvent> Events { get; set; }
            = new List<WarehouseSupplyItemEvent>();

        [NotMapped]
        public override string Unit => Supply.Unit;

        [NotMapped]
        public override decimal UnitValue => Supply.Price;
    }
}
