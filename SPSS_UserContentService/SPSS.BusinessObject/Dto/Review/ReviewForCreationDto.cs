using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Review;

public class ReviewForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.ProductItemIdIsRequired)]
    public Guid ProductItemId { get; set; }

    public List<string> ReviewImages { get; set; } = new List<string>();

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.RatingValueIsRequired)]
    [Range(0, 5, ErrorMessage = ExceptionMessageConstants.Validation.InvalidRatingRange)]
    public float RatingValue { get; set; }

    [StringLength(1000, ErrorMessage = ExceptionMessageConstants.Validation.CommentTooLong)]
    public string? Comment { get; set; }
}
