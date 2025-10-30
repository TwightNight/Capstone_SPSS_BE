using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Review;

public class ReviewForCreationDto
{
    [Required(ErrorMessage = "Product item ID is required.")]
    public Guid ProductItemId { get; set; }

    // Note: To validate that each string is a valid URL and to limit the count,
    // a custom validation attribute would be required.
    public List<string> ReviewImages { get; set; } = new List<string>();

    [Required(ErrorMessage = "Rating value is required.")]
    [Range(0, 5, ErrorMessage = "Rating value must be between 0 and 5.")]
    public float RatingValue { get; set; }

    [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
    public string? Comment { get; set; } // Changed to nullable to be explicit
}