using AutoMapper;
using SPSS.BusinessObject.Dto.SkinType;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings
{
	public class SkinTypeMappingProfile : Profile
	{
		public SkinTypeMappingProfile()
        {
            CreateMap<SkinType, SkinTypeWithDetailDto>();
            CreateMap<SkinType, SkinTypeDto>(); 

            CreateMap<SkinTypeForCreationDto, SkinType>();
            CreateMap<SkinTypeForUpdateDto, SkinType>();
        }
    }
}
