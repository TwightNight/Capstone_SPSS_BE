using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.SkinType
{
    public class CreateSkinTypeRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.SkinType.NameRequired)]
        [StringLength(255, ErrorMessage = ExceptionMessageConstants.SkinType.NameTooLong)]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = ExceptionMessageConstants.SkinType.DescriptionTooLong)]
        public string Description { get; set; }
    }
}
