using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Country;

public class CountryForCreationDto
{
    [Required]
    [StringLength(10)]
    public string CountryCode { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string CountryName { get; set; } = null!;
}
