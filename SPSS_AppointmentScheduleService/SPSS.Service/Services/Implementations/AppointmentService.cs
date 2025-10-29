using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SPSS.BusinessObject.DTOs;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
	public class AppointmentService : IAppointmentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAppointmentRepository _appointmentRepository;
		private readonly IMapper _mapper;

		public AppointmentService(
			IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_appointmentRepository = _unitOfWork.GetRepository<IAppointmentRepository>();
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<AppointmentResponseDto?> GetByIdAsync(Guid id)
		{
			var appointment = await _appointmentRepository.GetByIdAsync(id);

			if (appointment == null || appointment.IsDeleted)
				return null;

			return _mapper.Map<AppointmentResponseDto>(appointment);
		}

		public async Task<AppointmentResponseDto?> GetByIdIncludeAsync(
			Guid id,
			Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>? include = null)
		{
			var appointment = await _appointmentRepository.GetFirstOrDefaultAsync(
				predicate: a => a.Id == id && !a.IsDeleted,
				include: include
			);

			if (appointment == null)
				return null;

			return _mapper.Map<AppointmentResponseDto>(appointment);
		}

		public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
		{
			var appointments = await _appointmentRepository.GetAsync(
				filter: a => !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.CreatedAt)
			);

			return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
		}

		public async Task<IEnumerable<AppointmentResponseDto>> GetAsync(
			Expression<Func<Appointment, bool>>? filter = null,
			Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>? orderBy = null,
			Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>? include = null)
		{
			// Default filter: exclude deleted appointments
			Expression<Func<Appointment, bool>> finalFilter;

			if (filter == null)
			{
				finalFilter = a => !a.IsDeleted;
			}
			else
			{
				// Combine custom filter with IsDeleted filter
				var parameter = Expression.Parameter(typeof(Appointment), "a");
				var isDeletedCheck = Expression.Not(
					Expression.Property(parameter, nameof(Appointment.IsDeleted))
				);
				var customFilterBody = Expression.Invoke(filter, parameter);
				var combinedBody = Expression.AndAlso(isDeletedCheck, customFilterBody);
				finalFilter = Expression.Lambda<Func<Appointment, bool>>(combinedBody, parameter);
			}

			// Default order: by CreatedAt descending
			var finalOrderBy = orderBy ?? (q => q.OrderByDescending(a => a.CreatedAt));

			var appointments = await _appointmentRepository.GetAsync(
				filter: finalFilter,
				orderBy: finalOrderBy,
				include: include
			);

			return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
		}

		public async Task<PagedResponse<AppointmentResponseDto>> GetPagedAsync(int pageNumber, int pageSize)
		{
			var (items, totalCount) = await _appointmentRepository.GetPagedAsync(
				pageNumber: pageNumber,
				pageSize: pageSize,
				filter: a => !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.CreatedAt)
			);

			var mappedItems = _mapper.Map<IEnumerable<AppointmentResponseDto>>(items);

			return new PagedResponse<AppointmentResponseDto>
			(
				mappedItems,
				totalCount,
				pageNumber,
				pageSize
			);
		}

		public async Task<IEnumerable<AppointmentResponseDto>> GetByUserIdAsync(Guid userId)
		{
			var appointments = await _appointmentRepository.GetAsync(
				filter: a => a.UserId == userId && !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.StartDateTime)
			);

			return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
		}

		public async Task<IEnumerable<AppointmentResponseDto>> GetByStaffIdAsync(Guid staffId)
		{
			var appointments = await _appointmentRepository.GetAsync(
				filter: a => a.StaffId == staffId && !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.StartDateTime)
			);

			return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
		}

		public async Task<IEnumerable<AppointmentResponseDto>> GetByServiceIdAsync(Guid serviceId)
		{
			var appointments = await _appointmentRepository.GetAsync(
				filter: a => a.ServiceId == serviceId && !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.CreatedAt)
			);

			return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
		}

		public async Task<IEnumerable<AppointmentResponseDto>> GetByScheduleIdAsync(Guid scheduleId)
		{
			var appointments = await _appointmentRepository.GetAsync(
				filter: a => a.ScheduleId == scheduleId && !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.StartDateTime)
			);

			return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
		}

		public async Task<IEnumerable<AppointmentResponseDto>> GetBySessionIdAsync(Guid sessionId)
		{
			var appointments = await _appointmentRepository.GetAsync(
				filter: a => a.SessionId == sessionId && !a.IsDeleted,
				orderBy: q => q.OrderByDescending(a => a.StartDateTime)
			);

			return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
		}
	}
}