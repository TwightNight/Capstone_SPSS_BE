using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication
{
    public class PrivilegedRegisterRequest : RegisterRequest
    {
        [Required]
        [RegularExpression("^(Manager|Staff)$", ErrorMessage = "Role must be either 'Manager' or 'Staff'.")]
        public string RoleName { get; set; }
    }
}