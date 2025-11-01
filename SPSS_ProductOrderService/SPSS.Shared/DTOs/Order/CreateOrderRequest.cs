using SPSS.Shared.DTOs.OrderDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Order
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.Order.PaymentMethodIdRequired)]
        public Guid PaymentMethodId { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Order.AddressIdRequired)]
        public Guid AddressId { get; set; }

        // A user-friendly voucher code is better than a GUID in a request
        public string VoucherCode { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = ExceptionMessageConstants.Order.OrderMustContainAtLeastOne)]
        public List<CreateOrderDetailRequest> OrderDetails { get; set; }
    }
}
