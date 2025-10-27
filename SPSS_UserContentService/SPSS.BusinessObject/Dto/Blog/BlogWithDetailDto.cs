using SPSS.BusinessObject.Dto.BlogSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Blog;

public class BlogWithDetailDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Thumbnail { get; set; }
    public string? Author { get; set; } // Tên tác giả (UserName)
    public DateTimeOffset? LastUpdatedAt { get; set; }
    public List<BlogSectionDto> Sections { get; set; } = new();
}
