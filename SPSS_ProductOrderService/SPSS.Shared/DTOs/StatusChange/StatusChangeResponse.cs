using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.StatusChange
{
    public class StatusChangeResponse
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Status { get; set; }
        public Guid OrderId { get; set; }
    }
}
