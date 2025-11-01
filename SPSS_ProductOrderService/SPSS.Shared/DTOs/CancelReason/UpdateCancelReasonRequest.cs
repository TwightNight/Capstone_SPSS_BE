using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.CancelReason
{
    public class UpdateCancelReasonRequest
    {
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Range(0, 100, ErrorMessage = "Refund rate must be between 0 and 100.")]
        public decimal? RefundRate { get; set; }
    }
}
