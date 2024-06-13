using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class Payment_Detail
    {
        public int PaymentDetail_Id { get; set; }
        public int InvoMaster_Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public float PaymentAmt { get; set; }
        public int Transaction_Id { get; set; }
        public string Payment_Method { get; set; } 
    }
}