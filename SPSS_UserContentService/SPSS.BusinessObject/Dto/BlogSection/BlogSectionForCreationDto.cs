using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.BlogSection;

public class BlogSectionForCreationDto
{
    [Required(ErrorMessage = "Content type is required.")]
    [StringLength(50, ErrorMessage = "Content type cannot exceed 50 characters.")]
    public string ContentType { get; set; } = null!;

    [Required(ErrorMessage = "Subtitle is required.")]
    [StringLength(200, ErrorMessage = "Subtitle cannot exceed 200 characters.")]
    public string Subtitle { get; set; } = null!;

    [Required(ErrorMessage = "Content is required.")]
    public string Content { get; set; } = null!;

    [Range(0, int.MaxValue, ErrorMessage = "Order must be a non-negative number.")]
    public int Order { get; set; }
}