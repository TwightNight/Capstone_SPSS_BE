using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class ChatHistoryRepository : RepositoryBase<ChatHistory, Guid>, IChatHistoryRepository
{
    private IQueryable<ChatHistory> ActiveHistories => Entities.Where(ch => !ch.IsDeleted);
    public ChatHistoryRepository(UserDBContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<ChatHistory>> GetRecentMessagesByUserIdAsync(Guid userId, int limit = 100)
    {
        return await ActiveHistories 
            .Where(ch => ch.UserId == userId)
            .OrderByDescending(ch => ch.Timestamp)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<ChatHistory>> GetBySessionIdAsync(string sessionId)
    {
        return await ActiveHistories 
            .Where(ch => ch.SessionId == sessionId)
            .OrderBy(ch => ch.Timestamp)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<ChatHistory>> GetMessagesFromRecentSessionsAsync(Guid userId, int maxSessions = 10)
    {
        var recentSessionIds = await ActiveHistories 
            .Where(ch => ch.UserId == userId)
            .GroupBy(ch => ch.SessionId)
            .OrderByDescending(g => g.Max(ch => ch.Timestamp))
            .Select(g => g.Key)
            .Take(maxSessions)
            .ToListAsync();

        return await ActiveHistories 
            .Where(ch => recentSessionIds.Contains(ch.SessionId))
            .OrderBy(ch => ch.SessionId)
            .ThenBy(ch => ch.Timestamp)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<ChatHistory>> GetByUserIdAndSessionIdAsync(Guid userId, string sessionId)
    {
        return await ActiveHistories 
            .Where(ch => ch.UserId == userId && ch.SessionId == sessionId)
            .OrderBy(ch => ch.Timestamp)
            .ToListAsync();
    }
}