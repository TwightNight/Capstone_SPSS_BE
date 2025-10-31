using AutoMapper;
using SPSS.Shared.DTOs.Slot;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings
{
	public class SlotProfile : Profile
	{
		public SlotProfile()
		{
			// CreateMap<Source, Destination>();
			CreateMap<Slot, SlotResponse>().ReverseMap();
			CreateMap<PostSlotRequest, Slot>().ReverseMap();
			CreateMap<PutSlotRequest, Slot>().ReverseMap();
		}
	}
}
