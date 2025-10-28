using AutoMapper;
using SPSS.BusinessObject.Dto.SkinCondition;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Responses;
using System;
using System.Linq;
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
				throw new ArgumentNullException(nameof(skinTypeForCreationDto), ExceptionMessageConstants.SkinCondition.SkinConditionDataNull);

			var skinCondition = _mapper.Map<SkinCondition>(skinTypeForCreationDto);
			skinCondition.CreatedBy = userId.ToString();
			skinCondition.LastUpdatedBy = userId.ToString();
			skinCondition.CreatedTime = DateTimeOffset.UtcNow;
			skinCondition.LastUpdatedTime = DateTimeOffset.UtcNow;

			try
			{
				_skinConditionRepository.Add(skinCondition);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(ExceptionMessageConstants.SkinCondition.FailedToSave, ex.Message), ex);
			}

			var dto = _mapper.Map<SkinConditionDto>(skinCondition);
			return dto;
		}

		public async Task DeleteAsync(Guid id)
		{
			var skinCondition = await _skinConditionRepository.GetByIdAsync(id);
			if (skinCondition == null)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinCondition.NotFound, id));

			try
			{
				skinCondition.IsDeleted = true;
				_skinConditionRepository.Update(skinCondition);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(ExceptionMessageConstants.SkinCondition.FailedToDelete, ex.Message), ex);
			}
		}

		public async Task<SkinConditionDto> GetByIdAsync(Guid id)
		{
			var skinCondition = await _skinConditionRepository.GetByIdAsync(id);
			if (skinCondition == null)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinCondition.NotFound, id));

			var dto = _mapper.Map<SkinConditionDto>(skinCondition);
			return dto;
		}

		public async Task<PagedResponse<SkinConditionDto>> GetPagedAsync(int pageNumber, int pageSize)
		{
			var (skincondition, totalCount) = await _skinConditionRepository.GetPagedAsync(pageNumber, pageSize, null);

			var skinConditionDtos = skincondition.Select(s => _mapper.Map<SkinConditionDto>(s)).ToList();

			return new PagedResponse<SkinConditionDto>(skinConditionDtos, totalCount, pageNumber, pageSize);
		}

		public async Task<SkinConditionDto> UpdateAsync(SkinConditionForUpdateDto? skinTypeForUpdateDto)
		{
			if (skinTypeForUpdateDto == null)
				throw new ArgumentNullException(nameof(skinTypeForUpdateDto), ExceptionMessageConstants.SkinCondition.SkinConditionDataNull);

			var existingSkinCondition = await _skinConditionRepository.GetByIdAsync(skinTypeForUpdateDto.Id);
			if (existingSkinCondition == null)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinCondition.NotFound, skinTypeForUpdateDto.Id));

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
