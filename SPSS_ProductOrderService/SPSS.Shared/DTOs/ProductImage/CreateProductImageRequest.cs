using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.ProductImage
{
    public class CreateProductImageRequest
    {
        [Required(ErrorMessage = "The image URL is required.")]
        [StringLength(500, ErrorMessage = "The image URL cannot exceed 500 characters.")]
        [Url(ErrorMessage = "The image URL must be a valid URL.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "The thumbnail status is required.")]
        public bool IsThumbnail { get; set; }
    }
}
