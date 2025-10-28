using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Transaction;

public class CreateTransactionDto
{
    [Required]
    public string TransactionType { get; set; }

    [Range(1000, double.MaxValue, ErrorMessage = "Amount must be at least 1000")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
}
