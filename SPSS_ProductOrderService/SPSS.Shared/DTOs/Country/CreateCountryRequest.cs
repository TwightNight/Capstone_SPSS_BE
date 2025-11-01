using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Country
{
    public class CreateCountryRequest
    {
        [Required(ErrorMessage = "Country code is required.")]
        [StringLength(10, ErrorMessage = "Country code cannot be longer than 10 characters.")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Country name is required.")]
        [StringLength(100, ErrorMessage = "Country name cannot be longer than 100 characters.")]
        public string CountryName { get; set; }
    }
}
