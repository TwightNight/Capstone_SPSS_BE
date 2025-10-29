using SPSS.BusinessObject.Dto.SkinType;
using SPSS.Shared.Responses;

namespace SPSS.Service.Services.Interfaces
{
    public interface ISkinTypeService
    {
        Task<SkinTypeWithDetailDto> GetByIdAsync(Guid id);
        Task<PagedResponse<SkinTypeDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<SkinTypeWithDetailDto> CreateAsync(SkinTypeForCreationDto skinTypeForCreationDto);
        Task<SkinTypeWithDetailDto> UpdateAsync(Guid skinTypeId, SkinTypeForUpdateDto skinTypeForUpdateDto);
        Task DeleteAsync(Guid id);
    }
}