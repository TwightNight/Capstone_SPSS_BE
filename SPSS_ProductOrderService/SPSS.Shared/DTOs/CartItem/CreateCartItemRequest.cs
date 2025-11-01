using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.CartItem
{
    public class CreateCartItemRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.CartItem.UserIdRequired)]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.CartItem.ProductIdRequired)]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.CartItem.QuantityRequired)]
        [Range(1, int.MaxValue, ErrorMessage = ExceptionMessageConstants.CartItem.QuantityAtLeastOne)]
        public int Quantity { get; set; }
    }
}
