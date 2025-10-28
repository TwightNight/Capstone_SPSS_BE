using AutoMapper;
using SPSS.BusinessObject.Dto.SkinType;
using SPSS.Service.Interfaces;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Service.Implementations;

public class SkinTypeService : ISkinTypeService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ISkinTypeRepository _skinTypeRepository;

	public SkinTypeService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_skinTypeRepository = _unitOfWork.GetRepository<ISkinTypeRepository>();
	}

	public async Task<SkinTypeWithDetailDto> GetByIdAsync(Guid id)
	{
		var skinType = await _skinTypeRepository.GetByIdAsync(id);

		if (skinType == null)
			throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinType.NotFound, id));

		var dto = new SkinTypeWithDetailDto
		{
			Id = skinType.Id,
			Name = skinType.Name,
			Description = skinType.Description
		};

		return dto;
	}

	public async Task<PagedResponse<SkinTypeDto>> GetPagedAsync(int pageNumber, int pageSize)
	{
		var (skinTypes, totalCount) = await _skinTypeRepository.GetPagedAsync(pageNumber, pageSize, null);

		var skinTypeDtos = skinTypes.Select(skinType => new SkinTypeDto
		{
			Id = skinType.Id,
			Name = skinType.Name,
		}).ToList();

		return new PagedResponse<SkinTypeDto>(skinTypeDtos, totalCount, pageNumber, pageSize);
	}

	public async Task<bool> CreateAsync(SkinTypeForCreationDto? skinTypeForCreationDto, Guid userId)
	{
		if (skinTypeForCreationDto is null)
			throw new ArgumentNullException(nameof(skinTypeForCreationDto), ExceptionMessageConstants.SkinType.SkinTypeDataNull);

		var skinType = new SkinType
		{
			Id = Guid.NewGuid(),
			Name = skinTypeForCreationDto.Name,
			Description = skinTypeForCreationDto.Description,
		};

		try
		{
			_skinTypeRepository.Add(skinType);
			await _unitOfWork.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			throw new Exception(string.Format(ExceptionMessageConstants.SkinType.FailedToCreate, ex.Message), ex);
		}
		return true;
	}

	public async Task<SkinTypeWithDetailDto> UpdateAsync(Guid skinTypeId, SkinTypeForUpdateDto skinTypeForUpdateDto)
	{
		if (skinTypeForUpdateDto is null)
			throw new ArgumentNullException(nameof(skinTypeForUpdateDto), ExceptionMessageConstants.SkinType.SkinTypeDataNull);

		var skinType = await _skinTypeRepository.GetByIdAsync(skinTypeId);
		if (skinType == null)
			throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinType.NotFound, skinTypeId));

		_mapper.Map(skinTypeForUpdateDto, skinType);
		await _unitOfWork.SaveChangesAsync();
		return _mapper.Map<SkinTypeWithDetailDto>(skinType);
	}
}
