using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IPurchaseRequisitionsService
    {
        Task<PurchaseRequisitionDto> CreateAsync(PurchaseRequisitionCreateDto dto, string userId);

        Task<PurchaseRequisitionDto?> GetAsync(int id);

        Task<IList<PurchaseRequisitionDto>> GetManyAsync();

        Task<PurchaseRequisitionDto> UpdateAsync(int id, PurchaseRequisitionUpdateDto dto, string userId);
    }
}
