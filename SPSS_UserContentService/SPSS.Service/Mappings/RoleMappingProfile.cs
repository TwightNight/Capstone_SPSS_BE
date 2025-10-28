using AutoMapper;
using SPSS.BusinessObject.Dto.Role;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Role, RoleDto>();
        CreateMap<RoleForCreationDto, Role>();
        CreateMap<RoleForUpdateDto, Role>();
    }
}
