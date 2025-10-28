using AutoMapper;
using SPSS.BusinessObject.Dto.Account;
using SPSS.BusinessObject.Dto.Authentication;
using SPSS.BusinessObject.Dto.Role;
using SPSS.BusinessObject.Dto.User;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings;

public class AuthenticationMappingProfile : Profile
{
    public AuthenticationMappingProfile()
    {
        CreateMap<User, AuthUserDto>()
            .ForMember(dest => dest.Role,
                       opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : string.Empty));

        CreateMap<RegisterRequest, UserForCreationDto>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<UserForCreationDto, User>();
        CreateMap<User, UserDto>();
    }
}
