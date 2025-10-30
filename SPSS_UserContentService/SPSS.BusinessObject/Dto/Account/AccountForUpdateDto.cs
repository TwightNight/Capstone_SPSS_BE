using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Account;

public class AccountForUpdateDto
{
    // Basic Information
    [Required(ErrorMessage = "User name is required.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Surname is required.")]
    [StringLength(100, ErrorMessage = "Surname cannot exceed 100 characters.")]
    public string SurName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email address is required.")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }

    [StringLength(500, ErrorMessage = "Avatar URL cannot exceed 500 characters.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    public string AvatarUrl { get; set; }


    // Personal/Dermatological Information
    public Guid? SkinTypeId { get; set; }
    public Guid? SkinConditionId { get; set; }

    [Range(1, 150, ErrorMessage = "Age must be between 1 and 150.")]
    public int? Age { get; set; }
    public DateTime? DoB { get; set; }

    [StringLength(1000, ErrorMessage = "Diet description cannot exceed 1000 characters.")]
    public string Diet { get; set; }

    [StringLength(1000, ErrorMessage = "Daily routine description cannot exceed 1000 characters.")]
    public string DailyRoutine { get; set; }

    [StringLength(1000, ErrorMessage = "Allergy information cannot exceed 1000 characters.")]
    public string Allergy { get; set; }

    [StringLength(1000, ErrorMessage = "Certificate information cannot exceed 1000 characters.")]
    public string Certificate { get; set; }
}