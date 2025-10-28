using SPSS.BusinessObject.Dto.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Interfaces
{
    public interface IReplyService
    {
        Task<ReplyDto> CreateAsync(Guid userId, ReplyForCreationDto replyDto);
        Task<ReplyDto> UpdateAsync(Guid userId, ReplyForUpdateDto replyDto, Guid id);
        Task DeleteAsync(Guid userId, Guid id);
    }
}

