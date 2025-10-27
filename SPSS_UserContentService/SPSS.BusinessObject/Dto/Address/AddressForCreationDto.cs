// BusinessObjects/Dto/Address/AddressForCreationDto.cs
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Address;

public class AddressForCreationDto
{
    [Required]
    public int CountryId { get; set; }

    [Required]
    [StringLength(200)]
    public string CustomerName { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    public bool IsDefault { get; set; }

    [Required]
    [StringLength(50)]
    public string StreetNumber { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string AddressLine1 { get; set; } = null!;

    [StringLength(200)]
    public string AddressLine2 { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Ward { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string Postcode { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Province { get; set; } = null!;
}