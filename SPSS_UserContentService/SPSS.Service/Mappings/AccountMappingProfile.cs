using AutoMapper;
using SPSS.BusinessObject.Dto.Account;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<User, AccountDto>()
            .ForMember(
                dest => dest.SkinType, 
                opt => opt.MapFrom(src => src.SkinType.Name) 
            );

        CreateMap<AccountForUpdateDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
