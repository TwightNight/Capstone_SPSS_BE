using AutoMapper;
using SPSS.Shared.DTOs.Holiday;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings
{
	public class HolidayProfile : Profile
	{
		public HolidayProfile()
		{
			// CreateMap<Source, Destination>();
			CreateMap<Holiday, HolidayResponseDto>().ReverseMap();
			CreateMap<PostHolidayRequest, Holiday>().ReverseMap();
			CreateMap<PutHolidayRequest, Holiday>().ReverseMap();
		}
	}
}
