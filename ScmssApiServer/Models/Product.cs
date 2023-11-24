using AutoMapper;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class Product : Goods
    {
        public ICollection<ProductionCostItem> ProductionCostItems { get; set; }
            = new List<ProductionCostItem>();

        [NotMapped]
        public decimal ProductionCost => ProductionCostItems.Sum(i => i.TotalCost);

        public ICollection<Supply> Supplies { get; set; }
            = new List<Supply>();

        public IList<ProductionOrder> ProductionOrders { get; set; }
            = new List<ProductionOrder>();

        public IList<ProductionOrderItem> ProductionOrderItems { get; set; }
            = new List<ProductionOrderItem>();

        public IList<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();

        public IList<SalesOrderItem> SalesOrderItems { get; set; }
            = new List<SalesOrderItem>();
    }

    public class ProductMp : Profile
    {
        public ProductMp()
        {
            CreateMap<Product, GoodsDto>();
        }
    }
}
