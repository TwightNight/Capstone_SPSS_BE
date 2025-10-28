using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.User;

public class UserForCreationDto
{
    // Thông tin định danh vai trò (bắt buộc khi tạo)
    [Required]
    public Guid? RoleId { get; set; }

    // Thông tin đăng nhập (bắt buộc khi tạo)
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; } // Mật khẩu sẽ được hash ở service
    [Required]
    public string Status { get; set; }

    // Thông tin cơ bản (bắt buộc khi tạo)
    [Required]
    public string SurName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }

    // Thông tin cá nhân/da liễu (tùy chọn khi tạo)
    public Guid? SkinTypeId { get; set; }
    public Guid? SkinConditionId { get; set; }
    public int? Age { get; set; }
    public DateTime? DoB { get; set; }
    public string? Diet { get; set; }
    public string? DailyRoutine { get; set; }
    public string? Allergy { get; set; }
    public string? Certificate { get; set; }
}
