using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Account;

public class AccountForUpdateDto
{
    // Thông tin cơ bản
    public string UserName { get; set; }
    public string SurName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
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
