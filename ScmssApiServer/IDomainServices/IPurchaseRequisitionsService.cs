using ScmssApiServer.DTOs;
using ScmssApiServer.Services;

namespace ScmssApiServer.IDomainServices
{
    public interface IPurchaseRequisitionsService
    {
        Task<PurchaseRequisitionDto> CreateAsync(PurchaseRequisitionCreateDto dto, Identity currentIdentity);

        Task<PurchaseRequisitionDto?> GetAsync(int id, Identity currentIdentity);

        Task<IList<PurchaseRequisitionDto>> GetManyAsync(Identity currentIdentity);

        Task<PurchaseRequisitionDto> UpdateAsync(int id, PurchaseRequisitionUpdateDto dto, Identity currentIdentity);
    }
}
