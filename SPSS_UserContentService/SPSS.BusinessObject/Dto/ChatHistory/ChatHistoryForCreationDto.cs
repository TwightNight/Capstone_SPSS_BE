using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.ChatHistory;

public class ChatHistoryForCreationDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string MessageContent { get; set; }

    [Required]
    public string SenderType { get; set; }

    [Required]
    public string SessionId { get; set; }
}
