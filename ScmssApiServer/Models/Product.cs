using AutoMapper;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a product for sales.
    /// </summary>
    public class Product : Goods
    {
        public decimal MiscCost { get; set; }
        public double NetWeight { get; set; }

        [NotMapped]
        public decimal ProductionCost => SupplyCost + MiscCost;

        public IList<ProductionOrderItem> ProductionOrderItems { get; set; }
            = new List<ProductionOrderItem>();

        public IList<ProductionOrder> ProductionOrders { get; set; }
            = new List<ProductionOrder>();

        [NotMapped]
        public decimal Profit => Price - MiscCost;

        public IList<SalesOrderItem> SalesOrderItems { get; set; }
            = new List<SalesOrderItem>();

        public IList<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();

        public ICollection<Supply> Supplies { get; set; }
                                            = new List<Supply>();

        [NotMapped]
        public decimal SupplyCost => SupplyCostItems.Sum(i => i.TotalCost) + MiscCost;

        public ICollection<ProductionSupplyCostItem> SupplyCostItems { get; set; }
                    = new List<ProductionSupplyCostItem>();
    }

    public class ProductMp : Profile
    {
        public ProductMp()
        {
            CreateMap<Product, GoodsDto>();
        }
    }
}
