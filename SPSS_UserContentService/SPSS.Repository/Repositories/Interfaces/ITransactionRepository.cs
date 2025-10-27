using SPSS.BusinessObject.Models;
using SPSS.Shared.Base.Interfaces;

namespace SPSS.Repository.Repositories.Interfaces;

public interface ITransactionRepository : IRepositoryBase<Transaction, Guid>
{
    Task<IReadOnlyCollection<Transaction>> GetPendingTransactionsAsync();
    Task<IReadOnlyCollection<Transaction>> GetTransactionsByUserIdAsync(Guid userId);
    Task<Transaction?> GetTransactionByIdAsync(Guid id); 
    Task<IReadOnlyCollection<Transaction>> GetTransactionsByStatusAsync(string status);
}