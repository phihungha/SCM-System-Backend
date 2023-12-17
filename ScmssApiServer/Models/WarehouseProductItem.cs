using AutoMapper;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class WarehouseProductItem : WarehouseItem<WarehouseProductItemEvent>
    {
        [NotMapped]
        public override bool IsActive => Product.IsActive;

        public Product Product { get; set; } = null!;
        public int ProductId { get; set; }

        [NotMapped]
        public override string Unit => Product.Unit;

        [NotMapped]
        public override decimal UnitValue => Product.Price;

        public WarehouseProductItemEvent IssueForSales(double orderQuantity, SalesOrder order)
        {
            Quantity -= orderQuantity;
            var warehouseEvent = new WarehouseProductItemEvent
            {
                Time = DateTime.UtcNow,
                Quantity = Quantity,
                Change = -orderQuantity,
                SalesOrder = order,
                SalesOrderId = order.Id,
                WarehouseProductItem = this,
                WarehouseProductItemProductId = ProductId,
                WarehouseProductItemProductionFacilityId = ProductionFacilityId,
            };
            Events.Add(warehouseEvent);
            return warehouseEvent;
        }

        public WarehouseProductItemEvent ReceiveFromProduction(double orderQuantity, ProductionOrder order)
        {
            Quantity += orderQuantity;
            var warehouseEvent = new WarehouseProductItemEvent
            {
                Time = DateTime.UtcNow,
                Quantity = Quantity,
                Change = orderQuantity,
                ProductionOrder = order,
                ProductionOrderId = order.Id,
                WarehouseProductItem = this,
                WarehouseProductItemProductId = ProductId,
                WarehouseProductItemProductionFacilityId = ProductionFacilityId,
            };
            Events.Add(warehouseEvent);
            return warehouseEvent;
        }

        public WarehouseProductItemEvent UpdateQuantityManually(double newQuantity)
        {
            double change = newQuantity - Quantity;
            Quantity = newQuantity;
            var warehouseEvent = new WarehouseProductItemEvent
            {
                Time = DateTime.UtcNow,
                Quantity = Quantity,
                Change = change,
                WarehouseProductItem = this,
                WarehouseProductItemProductId = ProductId,
                WarehouseProductItemProductionFacilityId = ProductionFacilityId,
            };
            Events.Add(warehouseEvent);
            return warehouseEvent;
        }
    }

    public class WarehouseProductItemMP : Profile
    {
        public WarehouseProductItemMP()
        {
            CreateMap<WarehouseProductItem, WarehouseProductItemDto>();
        }
    }
}
