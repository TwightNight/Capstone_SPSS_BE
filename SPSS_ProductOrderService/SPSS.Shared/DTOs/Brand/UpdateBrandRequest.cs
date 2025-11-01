using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Brand
{
    public class UpdateBrandRequest
    {
        [StringLength(100, ErrorMessage = "Brand name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot be longer than 500 characters.")]
        [Url(ErrorMessage = "A valid URL is required for the image.")]
        public string ImageUrl { get; set; }

        public int? CountryId { get; set; }
    }
}
