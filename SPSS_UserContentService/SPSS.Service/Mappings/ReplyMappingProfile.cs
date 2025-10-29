using AutoMapper;
using SPSS.BusinessObject.Dto.Reply;
using SPSS.BusinessObject.Models;

namespace SPSS.Service.Mappings;

public class ReplyMappingProfile : Profile
{
    public ReplyMappingProfile()
    {
        // 1. Map từ Model (Reply) sang DTO (ReplyDto)
        // Cấu hình này giả định Model 'Reply' của bạn
        // có một navigation property tên là 'User'
        CreateMap<Reply, ReplyDto>()
            .ForMember(dest => dest.UserName,
                       opt => opt.MapFrom(src => src.User.UserName)) // Lấy UserName từ User
            .ForMember(dest => dest.AvatarUrl,
                       opt => opt.MapFrom(src => src.User.AvatarUrl)); // Lấy AvatarUrl từ User

        // 2. Map từ DTO (Creation) sang Model (Reply)
        CreateMap<ReplyForCreationDto, Reply>();

        // 3. Map từ DTO (Update) sang Model (Reply)
        CreateMap<ReplyForUpdateDto, Reply>();
    }
}