using SPSS.BusinessObject.Models;
using SPSS.Shared.Base.Interfaces;

namespace SPSS.Repository.Repositories.Interfaces;

public interface IChatHistoryRepository : IRepositoryBase<ChatHistory, Guid>
{
    Task<IReadOnlyCollection<ChatHistory>> GetRecentMessagesByUserIdAsync(Guid userId, int limit = 100);

    Task<IReadOnlyCollection<ChatHistory>> GetBySessionIdAsync(string sessionId);

    Task<IReadOnlyCollection<ChatHistory>> GetMessagesFromRecentSessionsAsync(Guid userId, int maxSessions = 10);

    Task<IReadOnlyCollection<ChatHistory>> GetByUserIdAndSessionIdAsync(Guid userId, string sessionId);
}
