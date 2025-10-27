using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Address;

public class AddressForUpdateDto
{
    [Required]
    public int CountryId { get; set; }

    [Required]
    [StringLength(200)]
    public string CustomerName { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string StreetNumber { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string AddressLine1 { get; set; } = null!;

    [StringLength(200)]
    public string AddressLine2 { get; set; } = null!;

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