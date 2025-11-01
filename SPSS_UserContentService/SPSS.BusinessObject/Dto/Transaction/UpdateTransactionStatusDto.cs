using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Transaction;

public class UpdateTransactionStatusDto
{
    public Guid TransactionId { get; set; }

    [RegularExpression("^(Success|Failed|Pending)$", ErrorMessage = ExceptionMessageConstants.Validation.InvalidTransactionStatus)]
    public string Status { get; set; }
}
