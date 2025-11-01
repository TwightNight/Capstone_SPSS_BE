using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Voucher
{
    public class CreateVoucherRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.Voucher.CodeRequired)]
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.Voucher.CodeTooLong)]
        public string Code { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Voucher.DescriptionRequired)]
        [StringLength(500, ErrorMessage = ExceptionMessageConstants.Voucher.DescriptionTooLong)]
        public string Description { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Voucher.DiscountRateRequired)]
        [Range(0, 100, ErrorMessage = ExceptionMessageConstants.Voucher.DiscountRateOutOfRange)]
        public double DiscountRate { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Voucher.MinimumOrderValueRequired)]
        [Range(0, double.MaxValue, ErrorMessage = ExceptionMessageConstants.Voucher.MinimumOrderValuePositive)]
        public double MinimumOrderValue { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Voucher.StartDateRequired)]
        public DateTimeOffset StartDate { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Voucher.EndDateRequired)]
        public DateTimeOffset EndDate { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Voucher.UsageLimitRequired)]
        [Range(1, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Voucher.UsageLimitAtLeastOne)]
        public int UsageLimit { get; set; }
    }
}
