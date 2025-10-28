using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.ChatHistory;

public class ChatHistoryDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string MessageContent { get; set; }
    public string SenderType { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string SessionId { get; set; }
}
