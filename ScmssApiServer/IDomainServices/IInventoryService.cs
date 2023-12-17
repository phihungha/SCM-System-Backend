using ScmssApiServer.DTOs;
using ScmssApiServer.Services;

namespace ScmssApiServer.IDomainServices
{
    public interface IInventoryService
    {
        public Task<WarehouseProductItemDto?> GetProduct(int facilityId, int id, Identity identity);

        public Task<IList<ProductionOrderDto>> GetProductionOrdersForIssue(
            int facilityId, InventoryOrderQueryDto query, Identity identity);

        public Task<IList<ProductionOrderDto>> GetProductionOrdersForReceive(
            int facilityId, InventoryOrderQueryDto query, Identity identity);

        public Task<IList<WarehouseProductItemDto>?> GetProducts(
            int facilityId, SimpleQueryDto query, Identity identity);

        public Task<IList<PurchaseOrderDto>> GetPurchaseOrdersForReceive(
            int facilityId, InventoryOrderQueryDto query, Identity identity);

        public Task<IList<SalesOrderDto>> GetSalesOrdersForIssue(
            int facilityId, InventoryOrderQueryDto query, Identity identity);

        public Task<IList<WarehouseSupplyItemDto>> GetSupplies(
            int facilityId, SimpleQueryDto query, Identity identity);

        public Task<WarehouseSupplyItemDto?> GetSupply(int facilityId, int id, Identity identity);

        public Task<IList<WarehouseProductItemDto>> UpdateProducts(
            int facilityId, WarehouseUpdateDto<WarehouseProductItemInputDto> dto, Identity identity);

        public Task<IList<WarehouseSupplyItemDto>> UpdateSupplies(
            int facilityId, WarehouseUpdateDto<WarehouseSupplyItemInputDto> dto, Identity identity);
    }
}
