using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElectBill.Repository;
using ElectBill.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace ElectBill.Repository
{
    public class Data : InterfData
    {
        public string connectionString {  get; set; }

       // Utility cs = new Utility();
        public Data() {
            connectionString = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;
        }
        public void SaveBillDetails(BillDetail details)
        {
            SqlConnection con = new SqlConnection(connectionString);
            try {

                details.Total_Amount = details.Items.Sum(i => i.Price * i.Quantity);
            con.Open();
            SqlCommand cmd = new SqlCommand("saveBILLDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cust_Name", details.Cust_Name);
                cmd.Parameters.AddWithValue("@Mobile", details.Mobile);
                cmd.Parameters.AddWithValue("@Address", details.Address);
                cmd.Parameters.AddWithValue("@total_Amount", details.Total_Amount);

                SqlParameter outputpara = new SqlParameter();
                outputpara.DbType = DbType.Int32;
                outputpara.Direction = ParameterDirection.Output;
                outputpara.ParameterName = "@Id";
                cmd.Parameters.Add(outputpara);
                cmd.ExecuteNonQuery();
                int id = int.Parse(outputpara.Value.ToString());

                if(details.Items.Count > 0) {
                    SaveBillItems(details.Items,con,id);
                }

            }
            catch(Exception) { 
            throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void SaveBillItems(List<Items> items, SqlConnection con, int id)
        {
            try
            {
                string qry = "insert into BillItems(Prod_Name,Price,Quality,BillId) values";
                foreach (var item in items)
                {
                    qry += String.Format("('{0}',{1},{2},{3}),", item.Prod_Name,item.Price,item.Quantity,id);
                }
                qry = qry.Remove(qry.Length-1);
                SqlCommand cmd = new SqlCommand(qry,con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}