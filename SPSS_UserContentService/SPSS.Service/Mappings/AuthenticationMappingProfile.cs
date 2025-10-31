using AutoMapper;
using SPSS.BusinessObject.Dto.Account;
using SPSS.BusinessObject.Dto.Authentication;
using SPSS.BusinessObject.Dto.Role;
using SPSS.BusinessObject.Dto.User;
using SPSS.BusinessObject.Dto.VerifyOtp;
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

        CreateMap<UserDto, AuthUserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleName));

        CreateMap<EmailVerificationForCreationDto, EmailVerification>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.IsUsed, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.IsRevoked, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.Attempts, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));
    }
}
