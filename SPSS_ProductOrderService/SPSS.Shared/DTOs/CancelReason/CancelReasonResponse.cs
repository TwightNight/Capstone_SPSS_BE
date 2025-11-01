using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.CancelReason
{
    public class CancelReasonResponse
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public decimal RefundRate { get; set; }
    }
}
