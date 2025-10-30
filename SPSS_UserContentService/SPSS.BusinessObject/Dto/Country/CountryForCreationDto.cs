using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Country;

public class CountryForCreationDto
{
    [Required(ErrorMessage = "Country code is required.")]
    [StringLength(10, ErrorMessage = "Country code cannot exceed 10 characters.")]
    public string CountryCode { get; set; } = null!;

    [Required(ErrorMessage = "Country name is required.")]
    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
    public string CountryName { get; set; } = null!;
}