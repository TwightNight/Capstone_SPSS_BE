using AutoMapper;
using SPSS.BusinessObject.Dto.SkinCondition;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Implementations;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
	public class SkinConditionService : ISkinConditionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ISkinConditionRepository _skinConditionRepository;

		public SkinConditionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_skinConditionRepository = _unitOfWork.GetRepository<ISkinConditionRepository>();
		}

		public async Task<SkinConditionDto> CreateAsync(SkinConditionForCreationDto? skinTypeForCreationDto, Guid userId)
		{
			if (skinTypeForCreationDto is null)
			{
				throw new ArgumentNullException(nameof(skinTypeForCreationDto), "Skin condition data cannot be null.");
			}

			// Ánh xạ DTO sang thực thể
			var skinCondition = _mapper.Map<SkinCondition>(skinTypeForCreationDto);
			skinCondition.CreatedBy = userId.ToString();
			skinCondition.LastUpdatedBy = userId.ToString();
			skinCondition.CreatedTime = DateTimeOffset.UtcNow;
			skinCondition.LastUpdatedTime = DateTimeOffset.UtcNow;

			//Thêm và lưu thay đổi
			_skinConditionRepository.Add(skinCondition);
			await _unitOfWork.SaveChangesAsync();

			// Ánh xạ thực thể sang DTO để trả về
			var dto = _mapper.Map<SkinConditionDto>(skinCondition);
			return dto;

		}

		public async Task DeleteAsync(Guid id)
		{
			var skinCondition = await _skinConditionRepository.GetByIdAsync(id);
			if (skinCondition == null)
			{
				throw new KeyNotFoundException($"Skin condition with ID {id} not found.");
			}
			//soft delete
			skinCondition.IsDeleted = true;
			_skinConditionRepository.Update(skinCondition);
			await _unitOfWork.SaveChangesAsync();

		}

		public Task<SkinConditionDto> GetByIdAsync(Guid id)
		{
			var skinCondition = _skinConditionRepository.GetByIdAsync(id);
			if (skinCondition == null)
			{
				throw new KeyNotFoundException($"Skin condition with ID {id} not found.");
			}
			var dto = _mapper.Map<SkinConditionDto>(skinCondition);
			return Task.FromResult(dto);
		}

		public async Task<PagedResponse<SkinConditionDto>> GetPagedAsync(int pageNumber, int pageSize)
		{
			// lấy dữ liệu phân trang từ repository
			var (skincondition, totalCount) = await _skinConditionRepository
				.GetPagedAsync(pageNumber, pageSize, null);

			// ánh xạ danh sách thực thể sang DTO
			var skinConditionDtos = skincondition.Select
				(skinCondition => _mapper.Map<SkinConditionDto>(skinCondition)).ToList();

			// tạo và trả về phản hồi phân trang
			return new PagedResponse<SkinConditionDto>(skinConditionDtos, totalCount, pageNumber, pageSize);
		}

		public async Task<SkinConditionDto> UpdateAsync(SkinConditionForUpdateDto? skinTypeForUpdateDto)
		{
			if (skinTypeForUpdateDto == null)
			{
				throw new ArgumentNullException(nameof(skinTypeForUpdateDto), "Skin condition data cannot be null.");
			}

			var existingSkinCondition = await _skinConditionRepository.GetByIdAsync(skinTypeForUpdateDto.Id);
			if (existingSkinCondition == null)
			{
				throw new KeyNotFoundException($"Skin condition with ID {skinTypeForUpdateDto.Id} not found.");
			}

			// Cập nhật các thuộc tính
			// Chỉ cập nhật khi có giá trị mới
			if (skinTypeForUpdateDto.Name != null)
				existingSkinCondition.Name = skinTypeForUpdateDto.Name;

			if (skinTypeForUpdateDto.Description != null)
				existingSkinCondition.Description = skinTypeForUpdateDto.Description;

			if (skinTypeForUpdateDto.SeverityLevel.HasValue)
				existingSkinCondition.SeverityLevel = skinTypeForUpdateDto.SeverityLevel.Value;

			if (skinTypeForUpdateDto.IsChronic.HasValue)
				existingSkinCondition.IsChronic = skinTypeForUpdateDto.IsChronic.Value;

			_skinConditionRepository.Update(existingSkinCondition);
			await _unitOfWork.SaveChangesAsync();

			var dto = _mapper.Map<SkinConditionDto>(existingSkinCondition);
			return dto;

		}
	}
}
