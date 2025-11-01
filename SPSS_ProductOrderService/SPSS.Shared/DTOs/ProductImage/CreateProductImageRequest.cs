using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.ProductImage
{
    public class CreateProductImageRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.ProductImage.ImageUrlRequired)]
        [StringLength(500, ErrorMessage = ExceptionMessageConstants.ProductImage.ImageUrlTooLong)]
        [Url(ErrorMessage = ExceptionMessageConstants.ProductImage.ImageUrlInvalid)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.ProductImage.IsThumbnailRequired)]
        public bool IsThumbnail { get; set; }
    }
}
