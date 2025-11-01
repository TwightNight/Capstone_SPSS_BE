using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.OrderDetail
{
    public class CreateOrderDetailRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.OrderDetail.ProductIdRequired)]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.OrderDetail.QuantityRequired)]
        [Range(1, 100, ErrorMessage = ExceptionMessageConstants.OrderDetail.QuantityRange)]
        public int Quantity { get; set; }
    }
}
