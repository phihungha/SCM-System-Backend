using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class PurchaseRequisitionItem : TransOrderItem
    {
        public PurchaseRequisition PurchaseRequisition { get; set; } = null!;
        public Supply Supply { get; set; } = null!;
    }

    public class PurchaseRequisitionItemMp : Profile
    {
        public PurchaseRequisitionItemMp()
        {
            CreateMap<PurchaseRequisitionItem, PurchaseRequisitionItemDto>();
        }
    }
}
