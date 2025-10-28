using AutoMapper;
using SPSS.BusinessObject.Dto.Country;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        // Map Entity -> DTO
        CreateMap<Country, CountryDto>();

        // Map DTO -> Entity
        CreateMap<CountryForCreationDto, Country>();
        CreateMap<CountryForUpdateDto, Country>();
    }
}
