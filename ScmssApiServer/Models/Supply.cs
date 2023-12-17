using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class Supply : Goods
    {
        public const string ImageFolderKey = "supply-images";

        public override string ImageFolderKeyInstance => ImageFolderKey;

        public ICollection<ProductSupplyCostItem> ProductionCostItems { get; set; }
            = new List<ProductSupplyCostItem>();

        public ICollection<ProductionOrder> ProductionOrders { get; set; }
            = new List<ProductionOrder>();

        public ICollection<Product> Products { get; set; }
                    = new List<Product>();

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
            = new List<PurchaseOrderItem>();

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
            = new List<PurchaseOrder>();

        public ICollection<PurchaseRequisitionItem> PurchaseRequisitionItems { get; set; }
            = new List<PurchaseRequisitionItem>();

        public ICollection<PurchaseRequisition> PurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<ProductionOrderSupplyUsageItem> SupplyUsageItems { get; set; }
            = new List<ProductionOrderSupplyUsageItem>();

        public Vendor Vendor { get; set; } = null!;
        public int VendorId { get; set; }

        public ICollection<WarehouseSupplyItem> WarehouseSupplyItems { get; set; }
            = new List<WarehouseSupplyItem>();
    }

    public class SupplyMp : Profile
    {
        public SupplyMp()
        {
            CreateMap<Supply, SupplyDto>();
            CreateMap<SupplyInputDto, Supply>();
        }
    }
}
