using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.SkinCondition;

public class SkinConditionForUpdateDto
{
    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.SkinConditionNameTooLong)]
    public string Name { get; set; }

    [StringLength(1000, ErrorMessage = ExceptionMessageConstants.Validation.DescriptionTooLong1000)]
    public string? Description { get; set; }

    [Range(1, 10, ErrorMessage = ExceptionMessageConstants.Validation.InvalidSeverityLevelRange)]
    public int? SeverityLevel { get; set; }

    public bool? IsChronic { get; set; }
}