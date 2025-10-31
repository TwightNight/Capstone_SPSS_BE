using Microsoft.EntityFrameworkCore.Query;
using SPSS.Shared.DTOs.Holiday;
using SPSS.Shared.DTOs.Slot;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Interfaces
{
	public interface ISlotService
	{
		Task<SlotResponse?> GetByIdAsync(Guid id);
		Task<IEnumerable<SlotResponse>> GetAllAsync();
		Task<SlotResponse> CreateAsync(PostSlotRequest request, Guid userId);
		Task<SlotResponse?> UpdateAsync(Guid id, Guid userId,PutSlotRequest request);
		Task<bool> DeleteAsync(Guid id);
	}
}
