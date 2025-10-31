using AutoMapper;
using SPSS.Shared.DTOs.Template;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings
{
	public class TemplateProfile : Profile
	{
		public TemplateProfile()
		{
			// CreateMap<Source, Destination>();
			CreateMap<WorkTemplate, TemplateResponse>().ReverseMap();
		}
	}
}
