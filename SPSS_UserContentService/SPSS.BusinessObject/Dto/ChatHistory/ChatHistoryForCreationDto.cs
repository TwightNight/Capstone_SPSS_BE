using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.ChatHistory;

public class ChatHistoryForCreationDto
{
    [Required(ErrorMessage = "User ID is required.")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Message content is required.")]
    [StringLength(4000, ErrorMessage = "Message content cannot exceed 4000 characters.")]
    public string MessageContent { get; set; }

    [Required(ErrorMessage = "Sender type is required.")]
    [RegularExpression("^(User|Bot)$", ErrorMessage = "Sender type must be either 'User' or 'Bot'.")]
    public string SenderType { get; set; }

    [Required(ErrorMessage = "Session ID is required.")]
    [StringLength(100, ErrorMessage = "Session ID cannot exceed 100 characters.")]
    public string SessionId { get; set; }
}