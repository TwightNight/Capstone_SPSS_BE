using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.SkinType;

public class SkinTypeForUpdateDto
{
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.SkinTypeNameTooLong)]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = ExceptionMessageConstants.Validation.DescriptionTooLong500)]
    public string Description { get; set; }

    [StringLength(2000, ErrorMessage = ExceptionMessageConstants.Validation.RoutineDetailsTooLong)]
    public string Routine { get; set; }
}