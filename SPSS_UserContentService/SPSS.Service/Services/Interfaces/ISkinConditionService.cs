using SPSS.BusinessObject.Dto.SkinCondition;
using SPSS.BusinessObject.Dto.SkinType;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Interfaces
{
	public interface ISkinConditionService
	{
		Task<SkinConditionDto> GetByIdAsync(Guid id);
		Task<PagedResponse<SkinConditionDto>> GetPagedAsync(int pageNumber, int pageSize);
		Task<SkinConditionDto> CreateAsync(SkinConditionForCreationDto? skinTypeForCreationDto, Guid userId);
		Task<SkinConditionDto> UpdateAsync(SkinConditionForUpdateDto skinTypeForUpdateDto);
		Task DeleteAsync(Guid id);

	}
}
