using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}