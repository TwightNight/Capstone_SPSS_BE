using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Product
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.Product.NameRequired)]
        [StringLength(255, ErrorMessage = ExceptionMessageConstants.Product.NameTooLong)]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = ExceptionMessageConstants.Product.EnglishNameTooLong)]
        public string EnglishName { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Product.DescriptionRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Product.PriceRequired)]
        [Range(0, double.MaxValue, ErrorMessage = ExceptionMessageConstants.Product.PricePositive)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Product.MarketPriceRequired)]
        [Range(0, double.MaxValue, ErrorMessage = ExceptionMessageConstants.Product.MarketPricePositive)]
        public decimal MarketPrice { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Product.QuantityInStockRequired)]
        [Range(0, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Product.QuantityInStockNonNegative)]
        public int QuantityInStock { get; set; }

        public Guid? ProductStatusId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ProductCategoryId { get; set; }
    }
}
