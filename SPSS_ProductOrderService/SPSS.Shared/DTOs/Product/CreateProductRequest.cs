using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Product
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(255, ErrorMessage = "Product name cannot be longer than 255 characters.")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "English name cannot be longer than 255 characters.")]
        public string EnglishName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Market price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Market price must be a positive number.")]
        public decimal MarketPrice { get; set; }

        [Required(ErrorMessage = "Quantity in stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock cannot be negative.")]
        public int QuantityInStock { get; set; }

        public Guid? ProductStatusId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ProductCategoryId { get; set; }
    }
}
