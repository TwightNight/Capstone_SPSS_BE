using AutoMapper;
using SPSS.BusinessObject.Dto.SkinCondition;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings
{
	public class SkinConditionMappingProfile : Profile
	{
		public SkinConditionMappingProfile() 
		{
			CreateMap<SkinCondition, SkinConditionDto>().ReverseMap();
			CreateMap<SkinConditionForCreationDto, SkinCondition>();
		}
	}
}
