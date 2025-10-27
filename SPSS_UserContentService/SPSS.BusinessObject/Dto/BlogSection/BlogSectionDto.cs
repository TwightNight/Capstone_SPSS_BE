using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.BlogSection;

public class BlogSectionDto
{
    public Guid Id { get; set; }
    public string? ContentType { get; set; }
    public string? Subtitle { get; set; }
    public string? Content { get; set; } 
    public int Order { get; set; }
}
