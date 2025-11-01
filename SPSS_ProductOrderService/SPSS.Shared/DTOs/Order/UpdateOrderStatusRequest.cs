using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Order
{
    public class UpdateOrderStatusRequest
    {
        [Required(ErrorMessage = "Order status is required.")]
        [StringLength(100, ErrorMessage = "Status cannot be longer than 100 characters.")]
        public string Status { get; set; }

        // If cancelling, a reason might be required
        public Guid? CancelReasonId { get; set; }
    }
}
