using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class editBill
    {
        public int BillId { get; set; }
        public string cust_Name { get; set; }
        public string cust_Mobile { get; set; }
        public string cust_Address { get; set;}
        public string OID { get; set; }
    }
}