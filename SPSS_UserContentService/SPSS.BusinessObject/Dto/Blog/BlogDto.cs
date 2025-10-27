using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Blog;

public class BlogDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Thumbnail { get; set; }
    public DateTimeOffset? LastUpdatedTime { get; set; }
    public string AuthorName { get; set; } = string.Empty; // Map từ User
}
