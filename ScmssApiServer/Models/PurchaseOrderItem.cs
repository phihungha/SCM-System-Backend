using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class PurchaseOrderItem : OrderItem
    {
        public int SupplyId { get; set; }
        public Supply Supply { get; set; } = null!;
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public decimal Discount { get; set; }
        public decimal NetPrice { get; set; }
    }
}
