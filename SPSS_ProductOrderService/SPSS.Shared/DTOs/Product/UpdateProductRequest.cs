using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Product
{
    public class UpdateProductRequest
    {
        [StringLength(255, ErrorMessage = ExceptionMessageConstants.Product.NameTooLong)]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = ExceptionMessageConstants.Product.EnglishNameTooLong)]
        public string EnglishName { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = ExceptionMessageConstants.Product.PricePositive)]
        public decimal? Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = ExceptionMessageConstants.Product.MarketPricePositive)]
        public decimal? MarketPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Product.QuantityInStockNonNegative)]
        public int? QuantityInStock { get; set; }

        public Guid? ProductStatusId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ProductCategoryId { get; set; }
    }
}
