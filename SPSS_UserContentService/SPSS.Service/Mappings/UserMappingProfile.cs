using AutoMapper;
using SPSS.BusinessObject.Dto.Account;
using SPSS.BusinessObject.Dto.User;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // --- Dành cho User tự quản lý Profile (Account) ---

        CreateMap<User, AccountDto>()
            .ForMember(dest => dest.SkinTypeName,
                       opt => opt.MapFrom(src => src.SkinType != null ? src.SkinType.Name : null))
            .ForMember(dest => dest.SkinConditionName,
                       opt => opt.MapFrom(src => src.SkinCondition != null ? src.SkinCondition.Name : null));

        CreateMap<AccountForUpdateDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // --- Dành cho Admin quản lý User ---

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName,
                       opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null))
            .ForMember(dest => dest.SkinTypeName,
                       opt => opt.MapFrom(src => src.SkinType != null ? src.SkinType.Name : null))
            .ForMember(dest => dest.SkinConditionName,
                       opt => opt.MapFrom(src => src.SkinCondition != null ? src.SkinCondition.Name : null));

        CreateMap<UserForCreationDto, User>();
        CreateMap<UserForUpdateDto, User>();
    }
}
