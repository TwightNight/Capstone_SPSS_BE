using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.OrderDetail
{
    public class CreateOrderDetailRequest
    {
        [Required(ErrorMessage = "The product ID is required.")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "The quantity is required.")]
        [Range(1, 100, ErrorMessage = "The quantity for each product must be between 1 and 100.")]
        public int Quantity { get; set; }
    }
}
