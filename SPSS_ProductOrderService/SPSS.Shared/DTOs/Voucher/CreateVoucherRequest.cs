using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Voucher
{
    public class CreateVoucherRequest
    {
        [Required(ErrorMessage = "Voucher code is required.")]
        [StringLength(100, ErrorMessage = "Voucher code cannot be longer than 100 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Discount rate is required.")]
        [Range(0, 100, ErrorMessage = "Discount rate must be between 0 and 100.")]
        public double DiscountRate { get; set; }

        [Required(ErrorMessage = "Minimum order value is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum order value must be a positive number.")]
        public double MinimumOrderValue { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTimeOffset StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTimeOffset EndDate { get; set; }

        [Required(ErrorMessage = "Usage limit is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1.")]
        public int UsageLimit { get; set; }
    }
}
