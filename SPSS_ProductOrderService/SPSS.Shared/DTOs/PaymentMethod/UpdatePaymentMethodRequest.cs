using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.PaymentMethod
{
    public class UpdatePaymentMethodRequest
    {
        [StringLength(100, ErrorMessage = "The payment type cannot exceed 100 characters.")]
        public string PaymentType { get; set; }

        [StringLength(200, ErrorMessage = "The image URL cannot exceed 200 characters.")]
        [Url(ErrorMessage = "The image URL must be a valid URL.")]
        public string ImageUrl { get; set; }
    }
}
