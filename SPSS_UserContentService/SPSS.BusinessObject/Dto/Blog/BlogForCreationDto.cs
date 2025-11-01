using SPSS.BusinessObject.Dto.BlogSection;
using SPSS.Shared.Constants; // Thêm using này
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Blog;

public class BlogForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.TitleIsRequired)]
    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.TitleTooLong)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.DescriptionIsRequired)]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.ThumbnailUrlIsRequired)]
    [StringLength(500, ErrorMessage = ExceptionMessageConstants.Validation.ThumbnailUrlTooLong)]
    [Url(ErrorMessage = ExceptionMessageConstants.Validation.InvalidThumbnailUrl)]
    public string Thumbnail { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.BlogSectionsAreRequired)]
    [MinLength(1, ErrorMessage = ExceptionMessageConstants.Validation.AtLeastOneSectionIsRequired)]
    public List<BlogSectionForCreationDto> Sections { get; set; } = new();
}