using SPSS.Shared.Constants;
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
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.Brand.NameTooLong)]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = ExceptionMessageConstants.Brand.TitleTooLong)]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = ExceptionMessageConstants.Brand.DescriptionTooLong)]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = ExceptionMessageConstants.Brand.ImageUrlTooLong)]
        [Url(ErrorMessage = ExceptionMessageConstants.Brand.ImageUrlInvalid)]
        public string ImageUrl { get; set; }

        public int? CountryId { get; set; }
    }
}
