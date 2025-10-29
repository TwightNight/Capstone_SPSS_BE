using Microsoft.EntityFrameworkCore.Query;
using SPSS.BusinessObject.DTOs;
using SPSS.BusinessObject.Models;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Interfaces
{
	public interface IAppointmentService
	{
		Task<AppointmentResponseDto?> GetByIdAsync(Guid id);
		Task<AppointmentResponseDto?> GetByIdIncludeAsync(
			Guid id,
			Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>? include = null);
		Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
		Task<IEnumerable<AppointmentResponseDto>> GetAsync(
			Expression<Func<Appointment, bool>>? filter = null,
			Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>? orderBy = null,
			Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>? include = null);
		Task<PagedResponse<AppointmentResponseDto>> GetPagedAsync(int pageNumber, int pageSize);
		Task<IEnumerable<AppointmentResponseDto>> GetByUserIdAsync(Guid userId);
		Task<IEnumerable<AppointmentResponseDto>> GetByStaffIdAsync(Guid staffId);
		Task<IEnumerable<AppointmentResponseDto>> GetByServiceIdAsync(Guid serviceId);
		Task<IEnumerable<AppointmentResponseDto>> GetByScheduleIdAsync(Guid scheduleId);
		Task<IEnumerable<AppointmentResponseDto>> GetBySessionIdAsync(Guid sessionId);
	}
}
