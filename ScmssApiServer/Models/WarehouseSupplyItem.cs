using AutoMapper;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class WarehouseSupplyItem : WarehouseItem<WarehouseSupplyItemEvent>
    {
        [NotMapped]
        public override bool IsActive => Supply.IsActive;

        public Supply Supply { get; set; } = null!;
        public int SupplyId { get; set; }

        [NotMapped]
        public override string Unit => Supply.Unit;

        [NotMapped]
        public override decimal UnitValue => Supply.Price;

        public WarehouseSupplyItemEvent AddProductionIssueEvent(double orderQuantity, ProductionOrder order)
        {
            Quantity -= orderQuantity;
            var warehouseEvent = new WarehouseSupplyItemEvent
            {
                Time = DateTime.UtcNow,
                Quantity = Quantity,
                Change = -orderQuantity,
                ProductionOrder = order,
                ProductionOrderId = order.Id,
                WarehouseSupplyItem = this,
                WarehouseSupplyItemSupplyId = SupplyId,
                WarehouseSupplyItemProductionFacilityId = ProductionFacilityId,
            };
            Events.Add(warehouseEvent);
            return warehouseEvent;
        }

        public WarehouseSupplyItemEvent AddPurchaseReceiveEvent(double orderQuantity, PurchaseOrder order)
        {
            Quantity += orderQuantity;
            var warehouseEvent = new WarehouseSupplyItemEvent
            {
                Time = DateTime.UtcNow,
                Quantity = Quantity,
                Change = orderQuantity,
                PurchaseOrder = order,
                PurchaseOrderId = order.Id,
                WarehouseSupplyItem = this,
                WarehouseSupplyItemSupplyId = SupplyId,
                WarehouseSupplyItemProductionFacilityId = ProductionFacilityId,
            };
            Events.Add(warehouseEvent);
            return warehouseEvent;
        }

        public WarehouseSupplyItemEvent SetQuantityManually(double newQuantity)
        {
            double change = newQuantity - Quantity;
            Quantity = newQuantity;
            var warehouseEvent = new WarehouseSupplyItemEvent
            {
                Time = DateTime.UtcNow,
                Quantity = Quantity,
                Change = change,
                WarehouseSupplyItem = this,
                WarehouseSupplyItemSupplyId = SupplyId,
                WarehouseSupplyItemProductionFacilityId = ProductionFacilityId,
            };
            Events.Add(warehouseEvent);
            return warehouseEvent;
        }
    }

    public class WarehouseSupplyItemMP : Profile
    {
        public WarehouseSupplyItemMP()
        {
            CreateMap<WarehouseSupplyItem, WarehouseSupplyItemDto>();
        }
    }
}
