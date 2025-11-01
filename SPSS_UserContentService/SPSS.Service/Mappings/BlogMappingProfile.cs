using AutoMapper;
using SPSS.BusinessObject.Dto.Blog;
using SPSS.BusinessObject.Dto.BlogSection;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Mappings;

public class BlogMappingProfile : Profile
{
    public BlogMappingProfile()
    {
        // --- Section Mappings ---
        CreateMap<BlogSection, BlogSectionDto>();
        CreateMap<BlogSectionForCreationDto, BlogSection>();
        CreateMap<BlogSectionForUpdateDto, BlogSection>()
            // Bỏ qua Id khi map để tránh lỗi "Key is read-only"
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // --- Blog Mappings ---
        CreateMap<Blog, BlogDto>()
            .ForMember(dest => dest.AuthorName,
                       opt => opt.MapFrom(src => $"{src.User.SurName} {src.User.FirstName}"));

        CreateMap<Blog, BlogWithDetailDto>()
            .ForMember(dest => dest.Author,
                       opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.LastUpdatedAt,
                       opt => opt.MapFrom(src => src.LastUpdatedTime))
            .ForMember(dest => dest.Sections,
                       opt => opt.MapFrom(src => src.BlogSections.OrderBy(s => s.Order)));

        CreateMap<BlogForCreationDto, Blog>();
        CreateMap<BlogForUpdateDto, Blog>()
            // Bỏ qua mapping collection để xử lý thủ công (sync)
            .ForMember(dest => dest.BlogSections, opt => opt.Ignore());
    }
}
