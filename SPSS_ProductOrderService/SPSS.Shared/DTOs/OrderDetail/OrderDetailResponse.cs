using SPSS.Shared.DTOs.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.OrderDetail
{
    public class OrderDetailResponse
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // The price at the time of purchase
        public Guid ProductId { get; set; }

        // Enriched product data for client convenience
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
    }
}
