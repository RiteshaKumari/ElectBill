using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

 

namespace ElectBill.Models
{
    public class Items
    {
        public int Id { get; set; }
        [Required]
        public string Prod_Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Item_Index { get; set; } //forein key of BillDetail.id
       
    }
}