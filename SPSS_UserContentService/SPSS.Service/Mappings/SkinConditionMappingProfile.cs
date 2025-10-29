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
            // 1. Map MỘT CHIỀU từ Model sang DTO (cho các hàm Get)
            CreateMap<SkinCondition, SkinConditionDto>();

            // 2. Map MỘT CHIỀU từ CreationDto sang Model (cho hàm Create)
            CreateMap<SkinConditionForCreationDto, SkinCondition>();

            // 3. THÊM MAP NÀY: Map MỘT CHIỀU từ UpdateDto sang Model (cho hàm Update)
            CreateMap<SkinConditionForUpdateDto, SkinCondition>();
        }
    }
}
