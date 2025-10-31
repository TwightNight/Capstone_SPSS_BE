using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Brand
{
    public class CreateBrandRequest
    {
        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(100, ErrorMessage = "Brand name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Brand title is required.")]
        [StringLength(200, ErrorMessage = "Brand title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Brand description is required.")]
        [StringLength(500, ErrorMessage = "Brand description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image URL is required.")]
        [StringLength(500, ErrorMessage = "Image URL cannot be longer than 500 characters.")]
        [Url(ErrorMessage = "Please enter a valid URL for the image.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Country ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid country.")]
        public int CountryId { get; set; }
    }
}
