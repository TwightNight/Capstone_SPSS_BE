using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Country;

public class CountryDto
{
    public int Id { get; set; }
    public string CountryCode { get; set; } = null!;
    public string CountryName { get; set; } = null!;
}
