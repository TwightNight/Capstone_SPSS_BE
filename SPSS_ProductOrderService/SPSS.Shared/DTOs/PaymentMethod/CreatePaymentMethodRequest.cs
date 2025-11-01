using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.PaymentMethod
{
    public class CreatePaymentMethodRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.PaymentMethod.PaymentTypeRequired)]
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.PaymentMethod.PaymentTypeTooLong)]
        public string PaymentType { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.PaymentMethod.ImageUrlRequired)]
        [StringLength(200, ErrorMessage = ExceptionMessageConstants.PaymentMethod.ImageUrlTooLong)]
        [Url(ErrorMessage = ExceptionMessageConstants.PaymentMethod.ImageUrlInvalid)]
        public string ImageUrl { get; set; }
    }
}
