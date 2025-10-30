using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.SkinCondition;

public class SkinConditionForUpdateDto
{
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string? Name { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [Range(1, 10, ErrorMessage = "Severity level must be between 1 and 10.")]
    public int? SeverityLevel { get; set; }

    public bool? IsChronic { get; set; }
}