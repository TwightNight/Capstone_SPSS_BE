using SPSS.BusinessObject.Dto.Transaction;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPSS.Service.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto dto, Guid userId);
        Task<TransactionDto> GetTransactionByIdAsync(Guid id);
        Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId);
        Task<PagedResponse<TransactionDto>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string status = null);
        Task<TransactionDto> UpdateTransactionStatusAsync(UpdateTransactionStatusDto dto, string adminId);
        Task<string> GenerateQrCodeAsync(decimal amount, string description);
    }
}