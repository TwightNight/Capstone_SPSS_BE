using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Transaction;

public class UpdateTransactionStatusDto
{
    [Required(ErrorMessage = "Transaction ID is required.")]
    public Guid TransactionId { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    // Example: Enforce specific status types.
    // Adjust these values to match your system's statuses (e.g., "Completed", "Pending", "Failed").
    [RegularExpression("^(Success|Failed|Pending)$", ErrorMessage = "Status must be 'Success', 'Failed', or 'Pending'.")]
    public string Status { get; set; }
}
