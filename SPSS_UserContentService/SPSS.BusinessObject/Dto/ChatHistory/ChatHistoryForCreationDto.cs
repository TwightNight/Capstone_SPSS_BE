using SPSS.Shared.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.ChatHistory;

public class ChatHistoryForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.UserIdIsRequired)]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.MessageContentIsRequired)]
    [StringLength(4000, ErrorMessage = ExceptionMessageConstants.Validation.MessageContentTooLong)]
    public string MessageContent { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.SenderTypeIsRequired)]
    [RegularExpression("^(User|Bot)$", ErrorMessage = ExceptionMessageConstants.Validation.InvalidSenderType)]
    public string SenderType { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.SessionIdIsRequired)]
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.SessionIdTooLong)]
    public string SessionId { get; set; }
}