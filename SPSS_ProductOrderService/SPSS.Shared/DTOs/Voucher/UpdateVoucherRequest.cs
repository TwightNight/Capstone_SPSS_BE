using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Voucher
{
    public class UpdateVoucherRequest
    {
        [StringLength(100, ErrorMessage = "Voucher code cannot be longer than 100 characters.")]
        public string Code { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot be longer than 50 characters.")]
        public string Status { get; set; }

        [Range(0, 100, ErrorMessage = "Discount rate must be between 0 and 100.")]
        public double? DiscountRate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum order value must be a positive number.")]
        public double? MinimumOrderValue { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1.")]
        public int? UsageLimit { get; set; }
    }
}
