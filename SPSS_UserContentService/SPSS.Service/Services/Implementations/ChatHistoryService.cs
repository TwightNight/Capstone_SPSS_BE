using AutoMapper;
using SPSS.BusinessObject.Dto.ChatHistory;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces; // Using interface IChatHistoryRepository
using SPSS.Repository.UnitOfWork.Interfaces; // Using interface IUnitOfWork
using SPSS.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations;

public class ChatHistoryService : IChatHistoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChatHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ChatHistoryDto>> GetChatHistoryByUserIdAsync(Guid userId, int limit = 100)
    {
        var chatHistory = await _unitOfWork.GetRepository<IChatHistoryRepository>().GetRecentMessagesByUserIdAsync(userId, limit);
        return _mapper.Map<IEnumerable<ChatHistoryDto>>(chatHistory);
    }

    public async Task<IEnumerable<ChatHistoryDto>> GetChatSessionAsync(string sessionId)
    {
        var chatSession = await _unitOfWork.GetRepository<IChatHistoryRepository>().GetBySessionIdAsync(sessionId);
        return _mapper.Map<IEnumerable<ChatHistoryDto>>(chatSession);
    }

    public async Task<IEnumerable<string>> GetRecentSessionsIdsAsync(Guid userId, int maxSessions = 10)
    {
        var recentMessages = await _unitOfWork.GetRepository<IChatHistoryRepository>().GetMessagesFromRecentSessionsAsync(userId, maxSessions);
        return recentMessages
            .GroupBy(ch => ch.SessionId)
            .Select(g => g.Key)
            .ToList();
    }

    public async Task<ChatHistoryDto> SaveChatMessageAsync(ChatHistoryForCreationDto chatMessage)
    {
        var chatHistoryEntity = _mapper.Map<ChatHistory>(chatMessage);

        _unitOfWork.GetRepository<IChatHistoryRepository>().Add(chatHistoryEntity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ChatHistoryDto>(chatHistoryEntity);
    }

    public async Task<IEnumerable<ChatHistoryDto>> GetChatHistoryByUserIdAndSessionIdAsync(Guid userId, string sessionId)
    {
        var chatHistory = await _unitOfWork.GetRepository<IChatHistoryRepository>().GetByUserIdAndSessionIdAsync(userId, sessionId);
        return _mapper.Map<IEnumerable<ChatHistoryDto>>(chatHistory);
    }
}