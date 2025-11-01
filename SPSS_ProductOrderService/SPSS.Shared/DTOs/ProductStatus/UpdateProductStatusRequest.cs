using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.ProductStatus
{
    public class UpdateProductStatusRequest
    {
        [StringLength(100, ErrorMessage = "Status name cannot be longer than 100 characters.")]
        public string StatusName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
    }
}
