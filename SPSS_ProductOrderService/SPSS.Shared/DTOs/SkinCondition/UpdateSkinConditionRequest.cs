using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.SkinCondition
{
    public class UpdateSkinConditionRequest
    {
        [StringLength(200, ErrorMessage = ExceptionMessageConstants.SkinCondition.NameTooLong)]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = ExceptionMessageConstants.SkinCondition.DescriptionTooLong)]
        public string Description { get; set; }

        [Range(1, 10, ErrorMessage = ExceptionMessageConstants.SkinCondition.SeverityRange)]
        public int? SeverityLevel { get; set; }

        public bool? IsChronic { get; set; }
    }
}
