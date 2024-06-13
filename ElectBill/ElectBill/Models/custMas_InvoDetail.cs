using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class custMas_InvoDetail
    {
        public custMaster custMaster { get; set; }
        public Invoice_Detail InvoiceDetail { get; set; }
    }
}