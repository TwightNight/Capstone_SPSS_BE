using AutoMapper;
using SPSS.BusinessObject.Dto.SkinCondition;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Responses;
using System.Security;

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

        public async Task<SkinConditionDto> GetByIdAsync(Guid id)
        {
            var skinCondition = await _skinConditionRepository.GetByIdAsync(id);

            if (skinCondition == null || skinCondition.IsDeleted)
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinCondition.NotFound, id));

            return _mapper.Map<SkinConditionDto>(skinCondition);
        }

        public async Task<PagedResponse<SkinConditionDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var (skincondition, totalCount) = await _skinConditionRepository.GetPagedAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                filter: s => !s.IsDeleted,
                orderBy: q => q.OrderBy(s => s.Name)
            );

            var skinConditionDtos = _mapper.Map<List<SkinConditionDto>>(skincondition);
            return new PagedResponse<SkinConditionDto>(skinConditionDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<SkinConditionDto> CreateAsync(SkinConditionForCreationDto dto, Guid userId)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto), ExceptionMessageConstants.SkinCondition.SkinConditionDataNull);

            var nameExists = await _skinConditionRepository.ExistsAsync(s => s.Name == dto.Name && !s.IsDeleted);
            if (nameExists)
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.SkinCondition.NameAlreadyExists, dto.Name));

            var skinCondition = _mapper.Map<SkinCondition>(dto);

            skinCondition.CreatedBy = userId.ToString();
            skinCondition.LastUpdatedBy = userId.ToString();
            skinCondition.CreatedTime = DateTimeOffset.UtcNow;
            skinCondition.LastUpdatedTime = DateTimeOffset.UtcNow;
            skinCondition.IsDeleted = false;

            try
            {
                _skinConditionRepository.Add(skinCondition);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ExceptionMessageConstants.SkinCondition.FailedToSave, ex.Message), ex);
            }

            return _mapper.Map<SkinConditionDto>(skinCondition);
        }

        public async Task<SkinConditionDto> UpdateAsync(Guid id, SkinConditionForUpdateDto dto, Guid userId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), ExceptionMessageConstants.SkinCondition.SkinConditionDataNull);

            var existingSkinCondition = await _skinConditionRepository.GetByIdAsync(id);
            if (existingSkinCondition == null || existingSkinCondition.IsDeleted)
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinCondition.NotFound, id));

            var nameExists = await _skinConditionRepository.ExistsAsync(s => s.Name == dto.Name && s.Id != id && !s.IsDeleted);
            if (nameExists)
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.SkinCondition.NameAlreadyExists, dto.Name));

            _mapper.Map(dto, existingSkinCondition);

            existingSkinCondition.LastUpdatedBy = userId.ToString();
            existingSkinCondition.LastUpdatedTime = DateTimeOffset.UtcNow;

            _skinConditionRepository.Update(existingSkinCondition);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SkinConditionDto>(existingSkinCondition);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var skinCondition = await _skinConditionRepository.GetByIdAsync(id);
            if (skinCondition == null || skinCondition.IsDeleted)
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinCondition.NotFound, id));

            try
            {
                skinCondition.IsDeleted = true;
                skinCondition.DeletedBy = userId.ToString();
                skinCondition.DeletedTime = DateTimeOffset.UtcNow;

                _skinConditionRepository.Update(skinCondition);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.SkinCondition.InUseByUser, ex.Message));
            }
        }
    }
}