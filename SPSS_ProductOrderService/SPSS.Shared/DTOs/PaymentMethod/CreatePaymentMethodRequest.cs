using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.PaymentMethod
{
    public class CreatePaymentMethodRequest
    {
        [Required(ErrorMessage = "The payment type is required.")]
        [StringLength(100, ErrorMessage = "The payment type cannot exceed 100 characters.")]
        public string PaymentType { get; set; }

        [Required(ErrorMessage = "The image URL is required.")]
        [StringLength(200, ErrorMessage = "The image URL cannot exceed 200 characters.")]
        [Url(ErrorMessage = "The image URL must be a valid URL.")]
        public string ImageUrl { get; set; }
    }
}
