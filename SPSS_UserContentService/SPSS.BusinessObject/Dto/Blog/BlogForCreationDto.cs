using SPSS.BusinessObject.Dto.BlogSection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Blog;

public class BlogForCreationDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Thumbnail URL is required.")]
    [StringLength(500, ErrorMessage = "Thumbnail URL cannot exceed 500 characters.")]
    [Url(ErrorMessage = "The Thumbnail must be a valid URL.")]
    public string Thumbnail { get; set; } = null!;

    [Required(ErrorMessage = "Blog sections are required.")]
    [MinLength(1, ErrorMessage = "At least one section is required.")]
    public List<BlogSectionForCreationDto> Sections { get; set; } = new();
}