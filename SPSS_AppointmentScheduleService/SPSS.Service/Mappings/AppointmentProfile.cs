using AutoMapper;
using SPSS.BusinessObject.DTOs;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings
{
	public class AppointmentProfile : Profile
	{
		public AppointmentProfile()
		{
			CreateMap<Appointment, AppointmentResponseDto>();
		}
	}
}
