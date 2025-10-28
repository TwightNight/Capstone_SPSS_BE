using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.User;

public class UserForUpdateDto
{
    [Required]
    public Guid? RoleId { get; set; }
    [Required]
    public string Status { get; set; }

    [Required]
    public string UserName { get; set; }
    [Required]
    public string SurName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public string AvatarUrl { get; set; }

    // Thông tin cá nhân/da liễu
    public Guid? SkinTypeId { get; set; }
    public Guid? SkinConditionId { get; set; }
    public int? Age { get; set; }
    public DateTime? DoB { get; set; }
    public string Diet { get; set; }
    public string DailyRoutine { get; set; }
    public string Allergy { get; set; }
    public string Certificate { get; set; }
}
