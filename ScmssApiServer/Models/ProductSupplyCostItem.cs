using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class ProductSupplyCostItem
    {
        public Product Product { get; set; } = null!;
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public Supply Supply { get; set; } = null!;
        public int SupplyId { get; set; }

        [NotMapped]
        public decimal TotalCost => UnitCost * (decimal)Quantity;

        [NotMapped]
        public string Unit => Supply.Unit;

        [NotMapped]
        public decimal UnitCost => Supply.Price;
    }
}
