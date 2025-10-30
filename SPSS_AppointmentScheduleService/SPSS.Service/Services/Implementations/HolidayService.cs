using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using SPSS.BusinessObject.DTOs.Holiday;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Implementations;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
	public class HolidayService : IHolidayService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHolidayRepository _holidayRepository;
		private readonly IMapper _mapper;

		public HolidayService(
			IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_holidayRepository = _unitOfWork.GetRepository<IHolidayRepository>();
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<HolidayResponseDto?> GetByIdAsync(Guid id)
		{
			var holiday = await _holidayRepository.GetByIdAsync(id);

			if (holiday == null || holiday.IsDeleted)
				return null;

			return _mapper.Map<HolidayResponseDto>(holiday);
		}

		public async Task<HolidayResponseDto?> GetByIdAsync(
			Guid id,
			Func<IQueryable<Holiday>, IIncludableQueryable<Holiday, object>>? include = null)
		{
			var holiday = await _holidayRepository.GetFirstOrDefaultAsync(
				predicate: a => a.Id == id && !a.IsDeleted,
				include: include
			);

			if (holiday == null)
				throw new NotFoundException(string.Format(ExceptionMessageConstants.Holiday.NotFound, id));

			return _mapper.Map<HolidayResponseDto>(holiday);
		}

		public async Task<IEnumerable<HolidayResponseDto>> GetAllAsync()
		{
			var holiday = await _holidayRepository.GetAsync(
				filter: a => !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.CreatedAt)
			);

			return _mapper.Map<IEnumerable<HolidayResponseDto>>(holiday);
		}

		public async Task<IEnumerable<HolidayResponseDto>> GetAsync(
			Expression<Func<Holiday, bool>>? filter = null,
			Func<IQueryable<Holiday>, IOrderedQueryable<Holiday>>? orderBy = null,
			Func<IQueryable<Holiday>, IIncludableQueryable<Holiday, object>>? include = null)
		{
			// Default filter: exclude deleted Holidays
			Expression<Func<Holiday, bool>> finalFilter;

			if (filter == null)
			{
				finalFilter = a => !a.IsDeleted;
			}
			else
			{
				// Combine custom filter with IsDeleted filter
				var parameter = Expression.Parameter(typeof(Holiday), "a");
				var isDeletedCheck = Expression.Not(
					Expression.Property(parameter, nameof(Holiday.IsDeleted))
				);
				var customFilterBody = Expression.Invoke(filter, parameter);
				var combinedBody = Expression.AndAlso(isDeletedCheck, customFilterBody);
				finalFilter = Expression.Lambda<Func<Holiday, bool>>(combinedBody, parameter);
			}

			// Default order: by CreatedAt descending
			var finalOrderBy = orderBy ?? (q => q.OrderByDescending(a => a.CreatedAt));

			var holiday = await _holidayRepository.GetAsync(
				filter: finalFilter,
				orderBy: finalOrderBy,
				include: include
			);

			return _mapper.Map<IEnumerable<HolidayResponseDto>>(holiday);
		}

		public async Task<PagedResponse<HolidayResponseDto>> GetPagedAsync(int pageNumber, int pageSize)
		{
			var (items, totalCount) = await _holidayRepository.GetPagedAsync(
				pageNumber: pageNumber,
				pageSize: pageSize,
				filter: a => !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.CreatedAt)
			);

			var mappedItems = _mapper.Map<IEnumerable<HolidayResponseDto>>(items);

			return new PagedResponse<HolidayResponseDto>
			(
				mappedItems,
				totalCount,
				pageNumber,
				pageSize
			);
		}

		public async Task<HolidayResponseDto> CreateAsync(PostHolidayRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			// Kiểm tra trùng ngày nếu có truyền HolidayDate
			var exists = await _holidayRepository.ExistsAsync(h =>
				h.HolidayDate == request.HolidayDate &&
				!h.IsDeleted);

			if (exists)
				throw new BusinessRuleException
					(string.Format(ExceptionMessageConstants.Holiday.DuplicateDate, request.HolidayDate.ToString("yyyy-MM-dd")));

			var holiday = _mapper.Map<Holiday>(request);
			holiday.CreatedAt = DateTimeOffset.Now;
			holiday.IsDeleted = false;

			_holidayRepository.Add(holiday);
			await _unitOfWork.SaveChangesAsync();

			var response = _mapper.Map<HolidayResponseDto>(holiday);
			return response;
		}

		public async Task<HolidayResponseDto?> UpdateAsync(Guid id, PutHolidayRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var holiday = await _holidayRepository.GetByIdAsync(id);
			if (holiday == null || holiday.IsDeleted)
				throw new NotFoundException(string.Format(ExceptionMessageConstants.Holiday.NotFound, id));

			// Kiểm tra trùng ngày nếu có truyền HolidayDate mới
			if (request.HolidayDate.HasValue)
			{
				var exists = await _holidayRepository.ExistsAsync(h =>
					h.HolidayDate == request.HolidayDate.Value &&
					!h.IsDeleted);

				if (exists)
					throw new BusinessRuleException
						(string.Format(ExceptionMessageConstants.Holiday.DuplicateDate, request.HolidayDate.Value.ToString("yyyy-MM-dd")));

				holiday.HolidayDate = request.HolidayDate.Value;
			}

			if (!string.IsNullOrWhiteSpace(request.Description))
				holiday.Description = request.Description;

			_holidayRepository.Update(holiday);
			await _unitOfWork.SaveChangesAsync();

			var response = _mapper.Map<HolidayResponseDto>(holiday);
			return response;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var holiday = await _holidayRepository.GetByIdAsync(id);
			if (holiday == null || holiday.IsDeleted)
				throw new NotFoundException(string.Format(ExceptionMessageConstants.Holiday.NotFound, id));

			holiday.IsDeleted = true;
			_holidayRepository.Update(holiday);
			await _unitOfWork.SaveChangesAsync();
			return true;

		}
	}
}
