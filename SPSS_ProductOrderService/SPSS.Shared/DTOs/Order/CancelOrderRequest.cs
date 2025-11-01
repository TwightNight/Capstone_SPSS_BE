using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Order
{
    public class CancelOrderRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.Order.CancelReasonIdRequired)]
        public Guid CancelReasonId { get; set; }
    }
}
