using SPSS.Shared.DTOs.OrderDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Order
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "The payment method ID is required.")]
        public Guid PaymentMethodId { get; set; }

        [Required(ErrorMessage = "The shipping address ID is required.")]
        public Guid AddressId { get; set; }

        // A user-friendly voucher code is better than a GUID in a request
        public string VoucherCode { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "The order must contain at least one item.")]
        public List<CreateOrderDetailRequest> OrderDetails { get; set; }
    }
}
