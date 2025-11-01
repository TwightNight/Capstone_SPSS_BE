using SPSS.Shared.Constants; // Thêm using này
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.BlogSection;

public class BlogSectionForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.ContentTypeIsRequired)]
    [StringLength(50, ErrorMessage = ExceptionMessageConstants.Validation.ContentTypeTooLong)]
    public string ContentType { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.SubtitleIsRequired)]
    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.SubtitleTooLong)]
    public string Subtitle { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.ContentIsRequired)]
    public string Content { get; set; } = null!;

    [Range(0, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Validation.OrderMustBeNonNegative)]
    public int Order { get; set; }
}