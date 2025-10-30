using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Address;

public class AddressForUpdateDto
{
    [Required(ErrorMessage = "CountryId is required.")]
    public int CountryId { get; set; }

    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(200, ErrorMessage = "Customer name cannot exceed 200 characters.")]
    public string CustomerName { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; } = null!;

    // Lưu ý: IsDefault thường được xử lý thông qua một endpoint riêng
    // để tránh logic phức tạp, nên tôi sẽ không thêm nó vào đây
    // nếu bạn cần, bạn có thể thêm: public bool IsDefault { get; set; }

    [Required(ErrorMessage = "Street number is required.")]
    [StringLength(50, ErrorMessage = "Street number cannot exceed 50 characters.")]
    public string StreetNumber { get; set; } = null!;

    [Required(ErrorMessage = "AddressLine1 is required.")]
    [StringLength(200, ErrorMessage = "AddressLine1 cannot exceed 200 characters.")]
    public string AddressLine1 { get; set; } = null!;

    [StringLength(200, ErrorMessage = "AddressLine2 cannot exceed 200 characters.")]
    public string? AddressLine2 { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "Ward is required.")]
    [StringLength(100, ErrorMessage = "Ward cannot exceed 100 characters.")]
    public string Ward { get; set; } = null!;

    [Required(ErrorMessage = "Postcode is required.")]
    [StringLength(20, ErrorMessage = "Postcode cannot exceed 20 characters.")]
    public string Postcode { get; set; } = null!;

    [Required(ErrorMessage = "Province is required.")]
    [StringLength(100, ErrorMessage = "Province cannot exceed 100 characters.")]
    public string Province { get; set; } = null!;
}