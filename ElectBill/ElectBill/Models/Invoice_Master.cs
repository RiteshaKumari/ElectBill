using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class Invoice_Master
    {
        public int Invoice_Id { get; set; }
        public DateTime InvoDateTime { get; set; }
        public int custMast_Id { get; set; } 
        public string status { get; set; }

    }
}