using SPSS.BusinessObject.Dto.BlogSection;
using SPSS.Shared.Constants; // Thêm using này
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Blog;

public class BlogForUpdateDto
{
    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.TitleTooLong)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [StringLength(500, ErrorMessage = ExceptionMessageConstants.Validation.ThumbnailUrlTooLong)]
    [Url(ErrorMessage = ExceptionMessageConstants.Validation.InvalidThumbnailUrl)]
    public string Thumbnail { get; set; } = null!;

    [MinLength(1, ErrorMessage = ExceptionMessageConstants.Validation.AtLeastOneSectionIsRequired)]
    public List<BlogSectionForUpdateDto> Sections { get; set; } = new();
}