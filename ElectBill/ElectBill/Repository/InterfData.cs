using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectBill.Models;
using System.Data.SqlClient;

namespace ElectBill.Repository
{
    internal interface InterfData
    {
        void SaveBillDetails(BillDetail details);
        void SaveBillItems(List<Items> items, SqlConnection con, int id);

        List<BillDetail> GetAllDetails();  

        BillDetail GetoneBillDetail(int id);
    }
}
