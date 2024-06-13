//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using ElectBill.Repository;
//using ElectBill.Models;
//using System.Data;
//using System.Data.SqlClient;
//using System.Configuration;
//using System.Security.Cryptography.X509Certificates;
//using System.Web.UI.WebControls;
//using System.Web.Mvc;


//namespace ElectBill.Repository
//{
//    public class Data 
//    {
//        public string connectionString { get; set; }

//        // Utility cs = new Utility();
//        public Data()
//        {
//            connectionString = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;
//        }
//        public int SaveBillDetails(custMaster details)
//        {
//            SqlConnection con = new SqlConnection(connectionString);
//            try
//            {
//                int id;
//           //     details.Total_Amount = details.Items.Sum(i => i.Price * i.Quantity);
//                con.Open();
//                SqlCommand cmd = new SqlCommand("saveCustDetail", con);
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("@Cust_Name", details.Cust_Name);
//                cmd.Parameters.AddWithValue("@Mobile", details.Mobile);
//                cmd.Parameters.AddWithValue("@Address", details.Address);
//                cmd.Parameters.AddWithValue("@total_Amount", details.Total_Amount);

//                SqlParameter outputpara = new SqlParameter();
//                outputpara.DbType = DbType.Int32;
//                outputpara.Direction = ParameterDirection.Output;
//                outputpara.ParameterName = "@Id";
//                cmd.Parameters.Add(outputpara);
//                cmd.ExecuteNonQuery();
//                id = int.Parse(outputpara.Value.ToString());

//                if (details.Items.Count > 0)
//                {
//                    SaveBillItems(details.Items, con, id);


//                    return id;
//                }
//                else
//                {
//                    return 0;
//                }


//            }
//            catch (Exception)
//            {
//                throw;

//            }
//            finally
//            {
//                con.Close();

//            }

//        }



//        public void SaveBillItems(List<Items> items, SqlConnection con, int id)
//        {
//            try
//            {
//                string qry = "insert into BillItems(Prod_Name,Price,Quality,BillId) values";
//                foreach (var item in items)
//                {
//                    qry += String.Format("('{0}',{1},{2},{3}),", item.Prod_Name, item.Price, item.Quantity, id);
//                }
//                qry = qry.Remove(qry.Length - 1);
//                SqlCommand cmd = new SqlCommand(qry, con);
//                cmd.ExecuteNonQuery();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }

//public list<billdetail> getalldetails()
//{
//    list<billdetail> list = new list<billdetail>();
//    billdetail detail;
//    sqlconnection con = new sqlconnection(connectionstring);
//    try
//    {
//        con.open();
//        sqlcommand cmd = new sqlcommand("getallebilldetails", con);
//        cmd.commandtype = commandtype.storedprocedure;
//        sqldatareader reader = cmd.executereader();
//        while (reader.read())
//        {
//            detail = new billdetail();
//            detail.id = int.parse(reader["billdetail_id"].tostring());
//            detail.cust_name = reader["cust_name"].tostring();
//            detail.mobile = reader["mobile"].tostring();
//            detail.address = reader["address"].tostring();
//            detail.total_amount = int.parse(reader["total_amount"].tostring());
//            detail.datetime = reader["todaydate"].tostring();
//            detail.custpass = reader["custpass"].tostring();
//            list.add(detail);
//        }
//    }
//    catch (exception)
//    {
//        throw;

//    }
//    finally { con.close(); }
//    return list;
//}

//        public List<BillDetail> GetAlllists()
//        {
//            List<BillDetail> list = new List<BillDetail>();
//            BillDetail Detail;
//            SqlConnection con = new SqlConnection(connectionString);
//            try
//            {
//                con.Open();
//                SqlCommand cmd = new SqlCommand("getAllEbillLists", con);
//                cmd.CommandType = CommandType.StoredProcedure;
//                SqlDataReader reader = cmd.ExecuteReader();
//                while (reader.Read())
//                {
//                    Detail = new BillDetail();
//                    Detail.Id = int.Parse(reader["BillDetail_Id"].ToString());
//                    Detail.Cust_Name = reader["Cust_Name"].ToString();
//                    Detail.Mobile = reader["Mobile"].ToString();
//                    Detail.Address = reader["Address"].ToString();
//                    Detail.Total_Amount = int.Parse(reader["Total_Amount"].ToString());
//                    Detail.dateTime = reader["todaydate"].ToString();
//                    Detail.custpass = reader["custpass"].ToString();
//                    list.Add(Detail);
//                }
//            }
//            catch (Exception)
//            {
//                throw;

//            }
//            finally { con.Close(); }
//            return list;
//        }

//public BillDetail GetoneBillDetail(int id)
//{
//    SqlConnection con = new SqlConnection(connectionString);
//    BillDetail detail = new BillDetail();

//    try
//    {
//        con.Open();
//        SqlCommand cmd = new SqlCommand("getOneEbillDetails", con);
//        cmd.CommandType = CommandType.StoredProcedure;
//        cmd.Parameters.AddWithValue("@Id", id);
//        SqlDataReader reader = cmd.ExecuteReader();
//        //if (reader.HasRows)
//        //{

//        //}
//        while (reader.Read())
//        {
//            detail.Id = int.Parse(reader["BillId"].ToString());
//            detail.Cust_Name = reader["Cust_Name"].ToString();
//            detail.Mobile = reader["Mobile"].ToString();
//            detail.Address = reader["Address"].ToString();
//            detail.Total_Amount = int.Parse(reader["Total_Amount"].ToString());
//            Items item = new Items();
//            item.Id = int.Parse(reader["ItemId"].ToString());
//            item.Prod_Name = reader["Prod_Name"].ToString();
//            item.Price = int.Parse(reader["Price"].ToString());
//            item.Quantity = int.Parse(reader["Quality"].ToString());
//            detail.Items.Add(item);
//        }
//    }
//    catch (Exception)
//    {
//        throw;
//    }
//    finally
//    {
//        con.Close();
//    }
//    return detail;
//}

//public bool deletedata(int id)
//{

//    sqlconnection con = new sqlconnection(connectionstring);
//    sqlcommand cmd = new sqlcommand("delete_data", con);
//    cmd.commandtype = commandtype.storedprocedure;
//    cmd.parameters.addwithvalue("@id", id);
//    con.open();
//    int i = cmd.executenonquery();
//    con.close();
//    if (i > 0)
//    {
//        return true;
//    }
//    else
//    {
//        return false;
//    }
//}
//   }
//}