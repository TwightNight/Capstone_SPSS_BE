using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;
using SPSS.Shared.Constants;

namespace SPSS.Repository.Repositories.Implementations;

public class TransactionRepository : RepositoryBase<Transaction, Guid>, ITransactionRepository
{
    private IQueryable<Transaction> ActiveTransactions => Entities.Where(t => !t.IsDeleted);

    public TransactionRepository(UserDBContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Transaction>> GetPendingTransactionsAsync()
    {
        return await ActiveTransactions 
            .Where(t => t.Status == TransactionStatus.Pending)
            .Include(t => t.User)
            .OrderByDescending(t => t.CreatedTime)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
    {
        return await ActiveTransactions 
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedTime)
            .ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
    {
        return await ActiveTransactions 
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id); 
    }

    public async Task<IReadOnlyCollection<Transaction>> GetTransactionsByStatusAsync(string status)
    {
        return await ActiveTransactions
            .Where(t => t.Status == status) 
            .Include(t => t.User)
            .OrderByDescending(t => t.CreatedTime)
            .ToListAsync();
    }
}