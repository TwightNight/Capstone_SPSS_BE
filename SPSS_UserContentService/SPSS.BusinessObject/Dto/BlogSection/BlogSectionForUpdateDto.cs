using SPSS.Shared.Constants; // Thêm using này
using System;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.BlogSection;

public class BlogSectionForUpdateDto
{
    public Guid Id { get; set; } // The ID of the existing section, or Guid.Empty for a new one.

    [StringLength(50, ErrorMessage = ExceptionMessageConstants.Validation.ContentTypeTooLong)]
    public string ContentType { get; set; } = null!;

    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.SubtitleTooLong)]
    public string Subtitle { get; set; } = null!;

    public string Content { get; set; } = null!;

    [Range(0, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Validation.OrderMustBeNonNegative)]
    public int Order { get; set; }
}