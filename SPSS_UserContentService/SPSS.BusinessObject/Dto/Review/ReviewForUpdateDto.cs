using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Review;

public class ReviewForUpdateDto
{
    // Note: Further validation for URL format and count could be done here as well.
    public List<string> ReviewImages { get; set; } = new List<string>();

    [Required(ErrorMessage = "Rating value is required.")]
    [Range(0, 5, ErrorMessage = "Rating value must be between 0 and 5.")]
    public float RatingValue { get; set; }

    [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
    public string? Comment { get; set; } // Changed to nullable to be explicit
}