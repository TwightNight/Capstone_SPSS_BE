using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Product
{
    public class UpdateProductRequest
    {
        [StringLength(255, ErrorMessage = "Product name cannot be longer than 255 characters.")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "English name cannot be longer than 255 characters.")]
        public string EnglishName { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal? Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Market price must be a positive number.")]
        public decimal? MarketPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock cannot be negative.")]
        public int? QuantityInStock { get; set; }

        public Guid? ProductStatusId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ProductCategoryId { get; set; }
    }
}
