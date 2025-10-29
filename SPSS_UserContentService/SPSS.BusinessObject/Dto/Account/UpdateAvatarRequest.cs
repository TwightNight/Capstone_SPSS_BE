using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Account;

public class UpdateAvatarRequest
{
    [Required]
    public string AvatarUrl { get; set; } = null!;
}
