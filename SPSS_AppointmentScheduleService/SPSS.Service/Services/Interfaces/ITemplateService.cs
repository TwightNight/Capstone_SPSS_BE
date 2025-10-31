using Microsoft.EntityFrameworkCore.Query;
using SPSS.Shared.DTOs.Template;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Interfaces
{
	public interface ITemplateService
	{
		Task<TemplateResponse?> GetByIdAsync(Guid id);

		Task<IEnumerable<TemplateResponse>> GetAllAsync();

	}
}
