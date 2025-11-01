using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Reply;

public class ReplyForUpdateDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.ReplyContentIsRequired)]
    [StringLength(1000, ErrorMessage = ExceptionMessageConstants.Validation.ReplyContentTooLong)]
    public string ReplyContent { get; set; }
}