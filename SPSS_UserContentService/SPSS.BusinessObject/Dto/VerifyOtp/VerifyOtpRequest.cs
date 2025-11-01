using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.VerifyOtp;

public class VerifyOtpRequest
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.EmailIsRequired)]
    public string Email { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.OtpCodeIsRequired)]
    public string Code { get; set; }
}
