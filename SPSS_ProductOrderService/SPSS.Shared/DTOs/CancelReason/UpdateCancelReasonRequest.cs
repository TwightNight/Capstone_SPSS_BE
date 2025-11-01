using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.CancelReason
{
    public class UpdateCancelReasonRequest
    {
        [StringLength(500, ErrorMessage = ExceptionMessageConstants.CancelReason.DescriptionTooLong)]
        public string Description { get; set; }

        [Range(0, 100, ErrorMessage = ExceptionMessageConstants.CancelReason.RefundRateOutOfRange)]
        public decimal? RefundRate { get; set; }
    }
}
