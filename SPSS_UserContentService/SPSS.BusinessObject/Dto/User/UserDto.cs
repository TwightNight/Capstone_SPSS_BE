using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.User;

public class UserDto
{
    public Guid UserId { get; set; }

    // Thông tin định danh vai trò
    public Guid? RoleId { get; set; }
    public string RoleName { get; set; } // Flattened

    // Thông tin cơ bản
    public string UserName { get; set; }
    public string SurName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string AvatarUrl { get; set; }
    public string Status { get; set; } // Quan trọng cho Admin

    // Thông tin cá nhân/da liễu
    public Guid? SkinTypeId { get; set; }
    public string SkinTypeName { get; set; } // Flattened
    public Guid? SkinConditionId { get; set; }
    public string SkinConditionName { get; set; } // Flattened

    public int? Age { get; set; }
    public DateTime? DoB { get; set; }
    public string Diet { get; set; }
    public string DailyRoutine { get; set; }
    public string Allergy { get; set; }
    public string Certificate { get; set; }

    // Thông tin Audit (Quan trọng cho Admin)
    public bool IsDeleted { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public string LastUpdatedBy { get; set; }
    public DateTimeOffset? LastUpdatedTime { get; set; }
    public string DeletedBy { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
}