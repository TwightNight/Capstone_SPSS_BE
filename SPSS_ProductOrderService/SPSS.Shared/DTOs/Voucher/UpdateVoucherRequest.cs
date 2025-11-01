using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Voucher
{
    public class UpdateVoucherRequest
    {
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.Voucher.CodeTooLong)]
        public string Code { get; set; }

        [StringLength(500, ErrorMessage = ExceptionMessageConstants.Voucher.DescriptionTooLong)]
        public string Description { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot be longer than 50 characters.")] // optional: you can move this into constants if you want
        public string Status { get; set; }

        [Range(0, 100, ErrorMessage = ExceptionMessageConstants.Voucher.DiscountRateOutOfRange)]
        public double? DiscountRate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = ExceptionMessageConstants.Voucher.MinimumOrderValuePositive)]
        public double? MinimumOrderValue { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Voucher.UsageLimitAtLeastOne)]
        public int? UsageLimit { get; set; }
    }
}
