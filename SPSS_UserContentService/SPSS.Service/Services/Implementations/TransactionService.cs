using AutoMapper;
using BusinessObjects.Dto.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SPSS.BusinessObject.Dto.Transaction;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Base.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionService> _logger;
    private readonly IMapper _mapper;

    public TransactionService(
        IUnitOfWork unitOfWork,
        ILogger<TransactionService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto dto, Guid userId)
    {
        try
        {
            _logger.LogInformation("Creating transaction for user {UserId}", userId);

            var transaction = _mapper.Map<Transaction>(dto);

            transaction.UserId = userId;
            transaction.CreatedBy = userId.ToString();
            transaction.LastUpdatedBy = userId.ToString();

            // Set các trường [Required] về giá trị mặc định
            transaction.QrImageUrl = string.Empty;
            transaction.BankInformation = string.Empty;

            _unitOfWork.GetRepository<ITransactionRepository>().Add(transaction);
            await _unitOfWork.SaveChangesAsync();

            return await GetTransactionByIdAsync(transaction.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<TransactionDto> GetTransactionByIdAsync(Guid id)
    {
        try
        {
            var transaction = await _unitOfWork.GetRepository<ITransactionRepository>().GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with ID {id} not found");
            }
            return _mapper.Map<TransactionDto>(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transaction: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId)
    {
        try
        {
            var transactions = await _unitOfWork.GetRepository<ITransactionRepository>().GetTransactionsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user transactions: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<PagedResponse<TransactionDto>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string? status = null)
    {
        try
        {
            Expression<Func<Transaction, bool>> filter = string.IsNullOrEmpty(status)
                ? t => !t.IsDeleted
                : t => !t.IsDeleted && t.Status == status;

            var (items, totalCount) = await _unitOfWork.GetRepository<ITransactionRepository>().GetPagedAsync(
                pageNumber,
                pageSize,
                filter,
                q => q.OrderByDescending(t => t.CreatedTime),
                q => q.Include(t => t.User)
            );

            var transactionDtos = _mapper.Map<IEnumerable<TransactionDto>>(items);

            return new PagedResponse<TransactionDto>(
                transactionDtos,
                totalCount,
                pageNumber,
                pageSize
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paged transactions: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<TransactionDto> UpdateTransactionStatusAsync(UpdateTransactionStatusDto dto, string adminId)
    {
        try
        {
            if (dto.Status != TransactionStatus.Approved && dto.Status != TransactionStatus.Rejected)
            {
                throw new ArgumentException($"Status must be '{TransactionStatus.Approved}' or '{TransactionStatus.Rejected}'");
            }

            var transaction = await _unitOfWork.GetRepository<ITransactionRepository>().GetTransactionByIdAsync(dto.TransactionId);

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with ID {dto.TransactionId} not found");
            }

            transaction.Status = dto.Status;
            transaction.LastUpdatedBy = adminId;
            transaction.LastUpdatedTime = DateTimeOffset.UtcNow;

            if (dto.Status == TransactionStatus.Approved)
            {
                transaction.ApprovedBy = adminId;
                transaction.ApprovedTime = DateTimeOffset.UtcNow;
            }

            _unitOfWork.GetRepository<ITransactionRepository>().Update(transaction);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TransactionDto>(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transaction status: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}