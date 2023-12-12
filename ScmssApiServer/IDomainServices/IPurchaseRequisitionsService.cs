using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IPurchaseRequisitionsService
    {
        Task<PurchaseRequisitionDto> CreateAsync(PurchaseRequisitionCreateDto dto, string userId);

        Task<PurchaseRequisitionDto?> GetAsync(int id, string userId);

        Task<IList<PurchaseRequisitionDto>> GetManyAsync(string userId);

        Task<PurchaseRequisitionDto> UpdateAsync(int id, PurchaseRequisitionUpdateDto dto, string userId);
    }
}
