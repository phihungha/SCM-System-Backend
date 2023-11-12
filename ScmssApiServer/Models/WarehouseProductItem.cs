using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class WarehouseProductItem : WarehouseItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [NotMapped]
        public override string Unit => Product.Unit;

        [NotMapped]
        public override decimal UnitValue => Product.Price;

        public ICollection<WarehouseProductItem> WarehouseProductItems { get; set; }
            = new List<WarehouseProductItem>();
    }
}
