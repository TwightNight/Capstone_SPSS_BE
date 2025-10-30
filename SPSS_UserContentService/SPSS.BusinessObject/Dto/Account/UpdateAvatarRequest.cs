using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Account;

public class UpdateAvatarRequest
{
    [Required(ErrorMessage = "Avatar URL is required.")]
    [StringLength(500, ErrorMessage = "Avatar URL cannot exceed 500 characters.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    public string AvatarUrl { get; set; } = null!;
}