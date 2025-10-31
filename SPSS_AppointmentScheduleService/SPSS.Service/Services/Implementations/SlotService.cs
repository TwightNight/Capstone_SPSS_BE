using AutoMapper;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.DTOs.Slot;
using SPSS.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
	public class SlotService : ISlotService
	{
		private IUnitOfWork _unitOfWork;
		private IMapper _mapper;
		private ISlotRepository _slotRepository;

		public SlotService(
			IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_slotRepository = _unitOfWork.GetRepository<ISlotRepository>();
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<SlotResponse?> GetByIdAsync(Guid id)
		{
			var slot = await _slotRepository.GetByIdAsync(id);
			if (slot == null || slot.IsDeleted)
				throw new NotFoundException(string.Format(ExceptionMessageConstants.Slot.NotFound, id));

			var response = _mapper.Map<SlotResponse>(slot);
			return response;
		}

		public async Task<IEnumerable<SlotResponse>> GetAllAsync()
		{
			var slots = await _slotRepository.GetAsync(
				filter: s => !s.IsDeleted,
				orderBy: q => q.OrderByDescending(s => s.CreatedTime)
			);
			var response = _mapper.Map<IEnumerable<SlotResponse>>(slots);
			return response;
		}

		public async Task<SlotResponse?> CreateAsync(PostSlotRequest request, Guid userId)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var existingSlot = await _slotRepository.ExistsAsync(s =>
				s.SlotMinutes == request.SlotMinutes && !s.IsDeleted
			);

			if (existingSlot)
				throw new BusinessRuleException
					(string.Format(ExceptionMessageConstants.Slot.DuplicateSlotMinutes, request.SlotMinutes));

			var slot = _mapper.Map<Slot>(request);
			slot.CreatedBy = userId.ToString();
			slot.CreatedTime = DateTimeOffset.Now;
			slot.IsDeleted = false;

			_slotRepository.Add(slot);
			await _unitOfWork.SaveChangesAsync();
			var response = _mapper.Map<SlotResponse>(slot);
			return response;
		}

		public async Task<SlotResponse?> UpdateAsync(Guid id, Guid userId, PutSlotRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));
			var slot = await _slotRepository.GetByIdAsync(id);
			if (slot == null || slot.IsDeleted)
				throw new NotFoundException(string.Format(ExceptionMessageConstants.Slot.NotFound, id));
			var existingSlot = await _slotRepository.ExistsAsync(s =>
				s.Id != id &&
				s.SlotMinutes == request.SlotMinutes &&
				!s.IsDeleted
			);
			if (existingSlot)
				throw new BusinessRuleException
					(string.Format(ExceptionMessageConstants.Slot.DuplicateSlotMinutes, request.SlotMinutes));

			slot.SlotMinutes = request.SlotMinutes;
			slot.LastUpdatedBy = userId.ToString();
			slot.LastUpdatedTime = DateTimeOffset.Now;
			_slotRepository.Update(slot);
			await _unitOfWork.SaveChangesAsync();
			var response = _mapper.Map<SlotResponse>(slot);
			return response;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var slot = await _slotRepository.GetByIdAsync(id);
			if (slot == null || slot.IsDeleted)
				throw new NotFoundException(string.Format(ExceptionMessageConstants.Slot.NotFound, id));
			slot.IsDeleted = true;
			_slotRepository.Update(slot);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}
	}
}
