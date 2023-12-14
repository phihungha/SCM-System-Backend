using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class WarehouseProductItem : WarehouseItem
    {
        public ICollection<WarehouseProductItem> Events { get; set; }
            = new List<WarehouseProductItem>();

        public Product Product { get; set; } = null!;
        public int ProductId { get; set; }

        [NotMapped]
        public override string Unit => Product.Unit;

        [NotMapped]
        public override decimal UnitValue => Product.Price;
    }
}
