using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Transaction;

public class CreateTransactionDto
{
    [Required(ErrorMessage = "Transaction type is required.")]
    // Example: Enforce specific transaction types for data integrity.
    // Adjust "Deposit" and "Withdrawal" to your actual required values.
    [RegularExpression("^(Deposit|Withdrawal)$", ErrorMessage = "Transaction type must be either 'Deposit' or 'Withdrawal'.")]
    public string TransactionType { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(1000, (double)decimal.MaxValue, ErrorMessage = "Amount must be at least 1000.")]
    public decimal Amount { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; } // Changed to nullable string for clarity
}
