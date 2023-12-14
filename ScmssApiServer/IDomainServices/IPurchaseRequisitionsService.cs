using ScmssApiServer.DTOs;
using ScmssApiServer.Services;

namespace ScmssApiServer.IDomainServices
{
    public interface IPurchaseRequisitionsService
    {
        Task<PurchaseRequisitionDto> CreateAsync(PurchaseRequisitionCreateDto dto, Identity identity);

        Task<PurchaseRequisitionDto?> GetAsync(int id, Identity identity);

        Task<IList<PurchaseRequisitionDto>> GetManyAsync(PurchaseRequisitionQueryDto dto, Identity identity);

        Task<PurchaseRequisitionDto> UpdateAsync(int id, PurchaseRequisitionUpdateDto dto, Identity identity);
    }
}
