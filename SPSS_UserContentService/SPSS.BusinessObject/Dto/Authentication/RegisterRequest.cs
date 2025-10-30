using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Authentication;

public class RegisterRequest
{
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Surname is required.")]
    [StringLength(100, ErrorMessage = "Surname cannot exceed 100 characters.")]
    public string SurName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string LastName { get; set; }
}