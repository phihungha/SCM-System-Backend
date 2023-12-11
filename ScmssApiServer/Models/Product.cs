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
        /// <summary>
        /// Production cost of this product = SupplyCost + MiscCost
        /// </summary>
        [NotMapped]
        public decimal Cost => SupplyCost + MiscCost;

        public decimal MiscCost { get; set; }
        public double NetWeight { get; set; }

        public IList<ProductionOrderItem> ProductionOrderItems { get; set; }
            = new List<ProductionOrderItem>();

        public IList<ProductionOrder> ProductionOrders { get; set; }
            = new List<ProductionOrder>();

        /// <summary>
        /// Profit of this product = Price - ProductionCost
        /// </summary>
        [NotMapped]
        public decimal Profit => Price - Cost;

        public IList<SalesOrderItem> SalesOrderItems { get; set; }
            = new List<SalesOrderItem>();

        public IList<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();

        public ICollection<Supply> Supplies { get; set; }
                                            = new List<Supply>();

        /// <summary>
        /// Cost of all supplies used for production of this product.
        /// </summary>
        [NotMapped]
        public decimal SupplyCost => SupplyCostItems.Sum(i => i.TotalCost) + MiscCost;

        /// <summary>
        /// Cost items of supplies used for production of this product.
        /// </summary>
        public ICollection<ProductSupplyCostItem> SupplyCostItems { get; set; }
                    = new List<ProductSupplyCostItem>();

        public ICollection<WarehouseProductItem> WarehouseProductItems { get; set; }
            = new List<WarehouseProductItem>();
    }

    public class ProductMp : Profile
    {
        public ProductMp()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Product, GoodsDto>();
            CreateMap<ProductInputDto, Product>();
        }
    }
}
