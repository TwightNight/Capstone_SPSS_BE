using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.BlogSection;

public class BlogSectionForUpdateDto
{
    public Guid Id { get; set; } // 0000-0000... nếu là section mới
    [Required]
    [StringLength(50)]
    public string ContentType { get; set; } = null!;
    [Required]
    [StringLength(200)]
    public string Subtitle { get; set; } = null!;
    [Required]
    public string Content { get; set; } = null!;
    public int Order { get; set; }
}
