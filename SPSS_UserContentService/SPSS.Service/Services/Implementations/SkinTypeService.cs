using AutoMapper;
using SPSS.BusinessObject.Dto.SkinType;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
    public class SkinTypeService : ISkinTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISkinTypeRepository _skinTypeRepository;
        private readonly IUserRepository _userRepository;

        public SkinTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _skinTypeRepository = _unitOfWork.GetRepository<ISkinTypeRepository>();
            _userRepository = _unitOfWork.GetRepository<IUserRepository>();
        }

        public async Task<SkinTypeWithDetailDto> GetByIdAsync(Guid id)
        {
            var skinType = await _skinTypeRepository.GetByIdAsync(id);

            if (skinType == null)
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinType.NotFound, id));

            return _mapper.Map<SkinTypeWithDetailDto>(skinType);
        }

        public async Task<PagedResponse<SkinTypeDto>> GetPagedAsync(int pageNumber, int pageSize)
        {

            (IEnumerable<SkinType> skinTypes, int totalCount) = await _skinTypeRepository.GetPagedAsync(
                pageNumber,
                pageSize,
                null, 
                orderBy: q => q.OrderBy(s => s.Name)
            );

            var skinTypeDtos = _mapper.Map<List<SkinTypeDto>>(skinTypes);
            return new PagedResponse<SkinTypeDto>(skinTypeDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<SkinTypeWithDetailDto> CreateAsync(SkinTypeForCreationDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto), ExceptionMessageConstants.SkinType.SkinTypeDataNull);

            var nameExists = await _skinTypeRepository.ExistsAsync(s => s.Name == dto.Name);
            if (nameExists)
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.SkinType.NameAlreadyExists, dto.Name));

            var skinType = _mapper.Map<SkinType>(dto);
            skinType.Id = Guid.NewGuid();


            try
            {
                _skinTypeRepository.Add(skinType);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ExceptionMessageConstants.SkinType.FailedToCreate, ex.Message), ex);
            }

            return _mapper.Map<SkinTypeWithDetailDto>(skinType);
        }

        public async Task<SkinTypeWithDetailDto> UpdateAsync(Guid skinTypeId, SkinTypeForUpdateDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto), ExceptionMessageConstants.SkinType.SkinTypeDataNull);

            var skinType = await _skinTypeRepository.GetByIdAsync(skinTypeId);
            if (skinType == null) 
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinType.NotFound, skinTypeId));

            var nameExists = await _skinTypeRepository.ExistsAsync(s => s.Name == dto.Name && s.Id != skinTypeId);
            if (nameExists)
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.SkinType.NameAlreadyExists, dto.Name));

            _mapper.Map(dto, skinType);


            _skinTypeRepository.Update(skinType);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SkinTypeWithDetailDto>(skinType);
        }

        public async Task DeleteAsync(Guid id)
        {
            var skinType = await _skinTypeRepository.GetByIdAsync(id);
            if (skinType == null) 
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.SkinType.NotFound, id));

           
            var isUsed = await _userRepository.ExistsAsync(u => u.SkinTypeId == id);
            if (isUsed)
            {
                
                throw new InvalidOperationException(ExceptionMessageConstants.SkinType.InUseByProduct); 
            }

            try
            {
                _skinTypeRepository.Delete(skinType);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ExceptionMessageConstants.SkinType.FailedToDelete, ex.Message), ex);
            }
        }
    }
}