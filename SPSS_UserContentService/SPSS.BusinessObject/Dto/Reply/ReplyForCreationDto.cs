using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System;

namespace SPSS.BusinessObject.Dto.Reply;

public class ReplyForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.ReviewIdIsRequired)]
    public Guid ReviewId { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.ReplyContentIsRequired)]
    [StringLength(1000, ErrorMessage = ExceptionMessageConstants.Validation.ReplyContentTooLong)]
    public string ReplyContent { get; set; }
}
