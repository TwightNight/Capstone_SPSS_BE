using SPSS.Shared.DTOs.CancelReason;
using SPSS.Shared.DTOs.OrderDetail;
using SPSS.Shared.DTOs.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Order
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public Guid AddressId { get; set; }

        // Simplified related data
        public string PaymentMethodName { get; set; }
        public string VoucherCode { get; set; }
        public string CancelReasonDescription { get; set; }

        // Collection of order line items
        public List<OrderDetailResponse> OrderDetails { get; set; }
    }
}
