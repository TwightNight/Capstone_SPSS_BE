using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication
{
    public class PrivilegedRegisterRequest : RegisterRequest
    {
        [Required]
        [RegularExpression("^(Manager|Staff)$", ErrorMessage = ExceptionMessageConstants.Validation.InvalidRoleForPrivilegedRegister)]
        public string RoleName { get; set; }
    }
}