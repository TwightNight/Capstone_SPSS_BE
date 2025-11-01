using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Transaction;

public class CreateTransactionDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.TransactionTypeIsRequired)]
    [RegularExpression("^(Deposit|Withdrawal)$", ErrorMessage = ExceptionMessageConstants.Validation.InvalidTransactionType)]
    public string TransactionType { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.AmountIsRequired)]
    [Range(1000, (double)decimal.MaxValue, ErrorMessage = ExceptionMessageConstants.Validation.AmountMustBeAtLeast1000)]
    public decimal Amount { get; set; }

    [StringLength(500, ErrorMessage = ExceptionMessageConstants.Validation.DescriptionTooLong500)]
    public string? Description { get; set; }
}
