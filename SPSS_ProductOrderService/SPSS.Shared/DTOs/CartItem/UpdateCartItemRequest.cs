using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.CartItem
{
    public class UpdateCartItemRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.CartItem.QuantityRequired)]
        [Range(1, int.MaxValue, ErrorMessage = ExceptionMessageConstants.CartItem.QuantityAtLeastOne)]
        public int Quantity { get; set; }
    }
}
