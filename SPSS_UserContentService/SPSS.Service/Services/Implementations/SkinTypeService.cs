using AutoMapper;
using SPSS.BusinessObject.Dto.SkinType;
using SPSS.Service.Interfaces;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Shared.Responses;

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
        // Truy vấn dữ liệu
        var skinType = await _skinTypeRepository.GetByIdAsync(id);

        if (skinType == null)
            throw new KeyNotFoundException($"SkinType with ID {id} not found.");

        // Ánh xạ thủ công
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
        // Retrieve paged SkinType entities and total count
        var (skinTypes, totalCount) = await _skinTypeRepository.GetPagedAsync(
            pageNumber, pageSize, null);

        // Manual mapping from SkinType to SkinTypeDto
        var skinTypeDtos = skinTypes.Select(skinType => new SkinTypeDto
        {
            Id = skinType.Id,
            Name = skinType.Name,
        }).ToList();

        // Create and return a paged response
        return new PagedResponse<SkinTypeDto>(skinTypeDtos, totalCount, pageNumber, pageSize);
    }

    public async Task<bool> CreateAsync(SkinTypeForCreationDto? skinTypeForCreationDto, Guid userId)
    {
        if (skinTypeForCreationDto is null)
            throw new ArgumentNullException(nameof(skinTypeForCreationDto), "SkinType data cannot be null.");

        // Map SkinType thủ công
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
            // Thông báo thành công nếu cần
            Console.WriteLine("SkinType đã được thêm thành công vào database.");
        }
        catch (Exception ex)
        {
            // Ghi log lỗi hoặc xử lý lỗi tại đây
            Console.WriteLine($"Đã xảy ra lỗi khi thêm SkinType: {ex.Message}");
            // Có thể ném lại ngoại lệ nếu muốn
            throw;
        }
        return true;
    }

    public async Task<SkinTypeWithDetailDto> UpdateAsync(Guid skinTypeId, SkinTypeForUpdateDto skinTypeForUpdateDto)
    {
        if (skinTypeForUpdateDto is null)
            throw new ArgumentNullException(nameof(skinTypeForUpdateDto), "SkinType data cannot be null.");

        var skinType = await _skinTypeRepository.GetByIdAsync(skinTypeId);
        if (skinType == null)
            throw new KeyNotFoundException($"SkinType with ID {skinTypeId} not found.");

        _mapper.Map(skinTypeForUpdateDto, skinType);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<SkinTypeWithDetailDto>(skinType);
    }

    //public async Task DeleteAsync(Guid id)
    //{
    //    var skinType = await _skinTypeRepository.GetByIdAsync(id);
    //    if (skinType == null)
    //        throw new KeyNotFoundException($"SkinType with ID {id} not found.");
    //    _skinTypeRepository.Update(skinType);
    //    await _unitOfWork.SaveChangesAsync();
    //}
}
