using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Blog;

public class BlogForUpdateDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    [StringLength(500)]
    public string Thumbnail { get; set; }  = null!;
    public List<BlogSectionForUpdateDto> Sections { get; set; } = new();
}
