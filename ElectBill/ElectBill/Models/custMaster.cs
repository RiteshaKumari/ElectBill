using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class custMaster
    {
        public int cust_Id { get; set; } 
        public string cust_Name { get; set; }

        public string cust_Mobile { get; set; }

        public string cust_Address { get; set;}

   

        public string password { get; set; }    
    }
}