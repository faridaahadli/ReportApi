using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Models
{
    public class PaymentResponseDto
    {
        public string PackageNumber { get; set; }

        public decimal PaymentAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
