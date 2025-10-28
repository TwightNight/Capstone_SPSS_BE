using SPSS.BusinessObject.Models;
using SPSS.Shared.Base.Interfaces;
// using SPSS.Shared.Enums; // <-- XÓA DÒNG NÀY

namespace SPSS.Repository.Repositories.Interfaces;

public interface ITransactionRepository : IRepositoryBase<Transaction, Guid>
{
    Task<IReadOnlyCollection<Transaction>> GetPendingTransactionsAsync();
    Task<IReadOnlyCollection<Transaction>> GetTransactionsByUserIdAsync(Guid userId);
    Task<Transaction?> GetTransactionByIdAsync(Guid id);

    // Sửa từ 'TransactionStatus status' về lại 'string status'
    Task<IReadOnlyCollection<Transaction>> GetTransactionsByStatusAsync(string status);
}