using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Account;

public class UpdateAvatarRequest
{
    [StringLength(500, ErrorMessage = ExceptionMessageConstants.Validation.AvatarUrlTooLong)]
    [Url(ErrorMessage = ExceptionMessageConstants.Validation.InvalidUrlFormat)]
    public string AvatarUrl { get; set; } = null!;
}