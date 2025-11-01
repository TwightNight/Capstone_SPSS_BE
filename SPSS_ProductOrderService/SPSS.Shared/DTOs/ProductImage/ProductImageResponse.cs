using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.ProductImage
{
    public class ProductImageResponse
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsThumbnail { get; set; }
    }
}
