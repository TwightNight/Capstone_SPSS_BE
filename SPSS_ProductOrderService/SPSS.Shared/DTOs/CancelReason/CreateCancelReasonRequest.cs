using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.CancelReason
{
    public class CreateCancelReasonRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.CancelReason.DescriptionRequired)]
        [StringLength(500, ErrorMessage = ExceptionMessageConstants.CancelReason.DescriptionTooLong)]
        public string Description { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.CancelReason.RefundRateRequired)]
        [Range(0, 100, ErrorMessage = ExceptionMessageConstants.CancelReason.RefundRateOutOfRange)]
        public decimal RefundRate { get; set; }
    }
}
