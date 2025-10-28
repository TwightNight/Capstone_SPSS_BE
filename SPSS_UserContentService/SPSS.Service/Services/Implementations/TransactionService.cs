using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing;
using System.IO;
using SPSS.Service.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.BusinessObject.Dto.Transaction;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Responses;
using SPSS.Shared.Constants;

namespace SPSS.Service.Implementations
{
	public class TransactionService : ITransactionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _configuration;
		private readonly ILogger<TransactionService> _logger;

		// Banking information read from configuration
		private readonly string _bankInformation;

		public TransactionService(
			IUnitOfWork unitOfWork,
			IConfiguration configuration,
			ILogger<TransactionService> logger)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_transactionRepository = _unitOfWork.GetRepository<ITransactionRepository>();
			_userRepository = _unitOfWork.GetRepository<IUserRepository>();

			var bankName = _configuration["Banking:BankName"] ?? "MBBANK";
			var accountNumber = _configuration["Banking:AccountNumber"] ?? "0358696560";
			var accountName = _configuration["Banking:AccountName"] ?? "NGUYEN NGOC SON";
			var branch = _configuration["Banking:Branch"] ?? "";

			_bankInformation = $"Ngân hàng: {bankName}\nSố tài khoản: {accountNumber}\nChủ tài khoản: {accountName}";
			if (!string.IsNullOrEmpty(branch))
			{
				_bankInformation += $"\nChi nhánh: {branch}";
			}
		}

		public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto dto, Guid userId)
		{
			try
			{
				_logger.LogInformation("Creating transaction for user {UserId}", userId);

				string qrCodeUrl;
				try
				{
					qrCodeUrl = await GenerateQrCodeAsync(dto.Amount, dto.Description);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "QR generation failed: {Message}", ex.Message);
					throw new Exception(string.Format(ExceptionMessageConstants.Transaction.FailedToGenerateQr, ex.Message), ex);
				}

				var transaction = new Transaction
				{
					Id = Guid.NewGuid(),
					UserId = userId,
					TransactionType = dto.TransactionType,
					Amount = dto.Amount,
					Status = "Pending",
					QrImageUrl = qrCodeUrl,
					BankInformation = _bankInformation,
					Description = dto.Description,
					CreatedBy = userId.ToString(),
					LastUpdatedBy = userId.ToString(),
					CreatedTime = DateTimeOffset.UtcNow,
					LastUpdatedTime = DateTimeOffset.UtcNow,
					IsDeleted = false
				};

				_transactionRepository.Add(transaction);
				await _unitOfWork.SaveChangesAsync();

				var user = await _userRepository.Entities
					.FirstOrDefaultAsync(u => u.UserId == userId);

				if (user == null)
					throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Transaction.UserNotFound, userId));

				return new TransactionDto
				{
					Id = transaction.Id,
					UserId = transaction.UserId,
					UserName = user.UserName,
					TransactionType = transaction.TransactionType,
					Amount = transaction.Amount,
					Status = transaction.Status,
					QrImageUrl = transaction.QrImageUrl,
					BankInformation = transaction.BankInformation,
					Description = transaction.Description,
					CreatedTime = transaction.CreatedTime,
					LastUpdatedTime = transaction.LastUpdatedTime,
					ApprovedTime = transaction.ApprovedTime
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating transaction: {ErrorMessage}", ex.Message);
				throw new Exception(string.Format(ExceptionMessageConstants.Transaction.FailedToCreate, ex.Message), ex);
			}
		}

		public async Task<string> GenerateQrCodeAsync(decimal amount, string description)
		{
			try
			{
				var bankName = _configuration["Banking:BankName"] ?? "MBBANK";
				var accountNumber = _configuration["Banking:AccountNumber"] ?? "0358696560";
				var accountName = _configuration["Banking:AccountName"] ?? "NGUYEN NGOC SON";
				var bankId = _configuration["Banking:BankId"] ?? "970422";

				string formattedAmount = ((long)amount).ToString();
				string normalizedDescription = RemoveVietnameseAccents(description);

				string qrContent = "00020101021138";
				qrContent += "540010A000000727012400";
				qrContent += $"06{bankId}";
				qrContent += $"01{accountNumber.Length:D2}{accountNumber}";
				qrContent += "0208QRIBFTTA";
				qrContent += "5303704";
				qrContent += $"54{formattedAmount.Length:D2}{formattedAmount}";
				qrContent += "5802VN";

				if (!string.IsNullOrEmpty(normalizedDescription))
				{
					qrContent += $"62{(normalizedDescription.Length + 4):D2}01{normalizedDescription.Length:D2}{normalizedDescription}";
				}

				qrContent += "6304";
				string checksum = CalculateChecksum(qrContent);
				qrContent = qrContent.Substring(0, qrContent.Length - 4) + "6304" + checksum;

				using (var qrGenerator = new QRCodeGenerator())
				{
					var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
					using (var qrCode = new BitmapByteQRCode(qrCodeData))
					{
						var qrCodeBytes = qrCode.GetGraphic(20);
						using var stream = new MemoryStream(qrCodeBytes);

						string fileName = $"qr-codes/payment-{Guid.NewGuid()}.png";
						// upload implementation omitted in this snippet. Return empty string or actual url after upload.
						//string imageUrl = await _firebaseImageService.UploadFileAsync(stream, fileName);
						string imageUrl = "";

						return imageUrl;
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error generating QR code: {ErrorMessage}", ex.Message);
				throw new Exception(string.Format(ExceptionMessageConstants.Transaction.FailedToGenerateQr, ex.Message), ex);
			}
		}

		private string CalculateChecksum(string qrContent)
		{
			ushort crc = 0xFFFF;
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(qrContent);

			foreach (byte b in bytes)
			{
				crc ^= (ushort)(b << 8);
				for (int i = 0; i < 8; i++)
				{
					if ((crc & 0x8000) > 0)
						crc = (ushort)((crc << 1) ^ 0x1021);
					else
						crc <<= 1;
				}
			}

			return crc.ToString("X4").ToLower();
		}

		private string RemoveVietnameseAccents(string text)
		{
			if (string.IsNullOrEmpty(text))
				return string.Empty;

			string[] vietnameseChars = new string[]
			{
				"áàảãạâấầẩẫậăắằẳẵặ", "ÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶ",
				"éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ",
				"íìỉĩị", "ÍÌỈĨỊ",
				"óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ",
				"úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ",
				"ýỳỷỹỵ", "ÝỲỶỸỴ",
				"đ", "Đ"
			};

			string[] replaceChars = new string[]
			{
				"a", "A",
				"e", "E",
				"i", "I",
				"o", "O",
				"u", "U",
				"y", "Y",
				"d", "D"
			};

			string result = text;

			for (int i = 0; i < vietnameseChars.Length; i++)
			{
				foreach (char c in vietnameseChars[i])
				{
					result = result.Replace(c.ToString(), replaceChars[i]);
				}
			}

			result = System.Text.RegularExpressions.Regex.Replace(result, @"[^a-zA-Z0-9\s]", "");

			return result;
		}

		public async Task<TransactionDto> GetTransactionByIdAsync(Guid id)
		{
			try
			{
				var transaction = await _transactionRepository.GetTransactionByIdAsync(id);

				if (transaction == null)
					throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Transaction.NotFound, id));

				return new TransactionDto
				{
					Id = transaction.Id,
					UserId = transaction.UserId,
					UserName = transaction.User.UserName,
					TransactionType = transaction.TransactionType,
					Amount = transaction.Amount,
					Status = transaction.Status,
					QrImageUrl = transaction.QrImageUrl,
					BankInformation = transaction.BankInformation,
					Description = transaction.Description,
					CreatedTime = transaction.CreatedTime,
					LastUpdatedTime = transaction.LastUpdatedTime,
					ApprovedTime = transaction.ApprovedTime
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting transaction: {ErrorMessage}", ex.Message);
				throw new Exception(string.Format(ExceptionMessageConstants.Transaction.FailedToGet, ex.Message), ex);
			}
		}

		public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId)
		{
			try
			{
				var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId);

				var user = await _userRepository.Entities
					.FirstOrDefaultAsync(u => u.UserId == userId);

				if (user == null)
					throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Transaction.UserNotFound, userId));

				return transactions.Select(t => new TransactionDto
				{
					Id = t.Id,
					UserId = t.UserId,
					UserName = user.UserName,
					TransactionType = t.TransactionType,
					Amount = t.Amount,
					Status = t.Status,
					QrImageUrl = t.QrImageUrl,
					BankInformation = t.BankInformation,
					Description = t.Description,
					CreatedTime = t.CreatedTime,
					LastUpdatedTime = t.LastUpdatedTime,
					ApprovedTime = t.ApprovedTime
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting user transactions: {ErrorMessage}", ex.Message);
				throw new Exception(string.Format(ExceptionMessageConstants.Transaction.FailedToGet, ex.Message), ex);
			}
		}

		public async Task<PagedResponse<TransactionDto>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string status = null)
		{
			try
			{
				IEnumerable<Transaction> transactions;
				if (!string.IsNullOrEmpty(status))
				{
					transactions = await _transactionRepository.GetTransactionsByStatusAsync(status);
				}
				else
				{
					transactions = await _transactionRepository.Entities
						.Where(t => !t.IsDeleted)
						.Include(t => t.User)
						.OrderByDescending(t => t.CreatedTime)
						.ToListAsync();
				}

				int totalCount = transactions.Count();

				var pagedTransactions = transactions
					.Skip((pageNumber - 1) * pageSize)
					.Take(pageSize)
					.ToList();

				var transactionDtos = pagedTransactions.Select(t => new TransactionDto
				{
					Id = t.Id,
					UserId = t.UserId,
					UserName = t.User?.UserName ?? "Unknown",
					TransactionType = t.TransactionType,
					Amount = t.Amount,
					Status = t.Status,
					QrImageUrl = t.QrImageUrl,
					BankInformation = t.BankInformation,
					Description = t.Description,
					CreatedTime = t.CreatedTime,
					LastUpdatedTime = t.LastUpdatedTime,
					ApprovedTime = t.ApprovedTime
				}).ToList();

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
				throw new Exception(string.Format(ExceptionMessageConstants.Transaction.FailedToGet, ex.Message), ex);
			}
		}

		public async Task<TransactionDto> UpdateTransactionStatusAsync(UpdateTransactionStatusDto dto, string adminId)
		{
			try
			{
				if (dto.Status != "Approved" && dto.Status != "Rejected")
				{
					throw new ArgumentException(string.Format(ExceptionMessageConstants.Validation.InvalidArgument, "Status must be 'Approved' or 'Rejected'"));
				}

				var transaction = await _transactionRepository.GetTransactionByIdAsync(dto.TransactionId);

				if (transaction == null)
					throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Transaction.NotFound, dto.TransactionId));

				transaction.Status = dto.Status;
				transaction.LastUpdatedBy = adminId;
				transaction.LastUpdatedTime = DateTimeOffset.UtcNow;

				if (dto.Status == "Approved")
				{
					transaction.ApprovedBy = adminId;
					transaction.ApprovedTime = DateTimeOffset.UtcNow;
				}

				_transactionRepository.Update(transaction);
				await _unitOfWork.SaveChangesAsync();

				return new TransactionDto
				{
					Id = transaction.Id,
					UserId = transaction.UserId,
					UserName = transaction.User?.UserName ?? "Unknown",
					TransactionType = transaction.TransactionType,
					Amount = transaction.Amount,
					Status = transaction.Status,
					QrImageUrl = transaction.QrImageUrl,
					BankInformation = transaction.BankInformation,
					Description = transaction.Description,
					CreatedTime = transaction.CreatedTime,
					LastUpdatedTime = transaction.LastUpdatedTime,
					ApprovedTime = transaction.ApprovedTime
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating transaction status: {ErrorMessage}", ex.Message);
				throw new Exception(string.Format(ExceptionMessageConstants.Transaction.FailedToUpdateStatus, ex.Message), ex);
			}
		}
	}
}
