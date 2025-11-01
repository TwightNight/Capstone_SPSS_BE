using SPSS.Shared.DTOs.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Product
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public int SoldCount { get; set; }
        public double Rating { get; set; }
        public decimal Price { get; set; }
        public decimal MarketPrice { get; set; }
        public int QuantityInStock { get; set; }
        public Guid? BrandId { get; set; }
        public string BrandName { get; set; } // Flattened property
        public Guid? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } // Flattened property
        public Guid? ProductStatusId { get; set; }
        public string ProductStatusName { get; set; } // Flattened property
        public List<ProductImageResponse> Images { get; set; }
    }
}
