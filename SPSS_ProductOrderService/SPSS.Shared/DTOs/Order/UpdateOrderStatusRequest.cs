using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Order
{
    public class UpdateOrderStatusRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.Order.StatusRequired)]
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.Order.StatusTooLong)]
        public string Status { get; set; }

        // If cancelling, a reason might be required
        public Guid? CancelReasonId { get; set; }
    }
}
