using Report.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Models
{
    public class OmsResponseDto
    {
        public string Pin { get; set; }
        public string Fullname { get; set; }
        public string Tpn { get; set; }
        public string OrganisationName { get; set; }
        public PackageStatusEnum PackageStatus { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public PackageTypeEnum PackageType { get; set; }
      
        public DateTime ContractDate { get; set; }
        public string PackageNumber { get; set; }
        public int? ApplicationCount { get; set; }
        public string? InvoiceNumber { get; set; }
        public bool IsOnline { get; set; }

    }
}
