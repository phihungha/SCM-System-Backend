using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class PurchaseRequisitionItem : OrderItem
    {
        public int SupplyId { get; set; }
        public Supply Supply { get; set; } = null!;
        public int PurchaseRequisitionId { get; set; }
        public PurchaseRequisition PurchaseRequisition { get; set; } = null!;
    }
}
