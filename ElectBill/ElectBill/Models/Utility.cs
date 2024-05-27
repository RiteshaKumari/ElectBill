using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class Utility
    {
        const string dbo_schema = "admin_kashishdb";//---------????????????
        private static object lockObject = new object();
        string CS = string.Empty;

        public Utility()
        {
            if (string.IsNullOrEmpty(CS))
            {
                CS = ConfigurationManager.ConnectionStrings["mycon"].ToString();
            }
        }

        #region DataSet
        public DataSet fn_DataSet(string procedure, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlDataAdapter tda = new SqlDataAdapter(dbo_schema + "." + procedure, con))
                {
                    tda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter param in sqlParameters)
                    {
                        tda.SelectCommand.Parameters.Add(param);
                    }
                    DataSet DS = new DataSet();
                    lock (lockObject)
                    {
                        tda.Fill(DS);
                    }

                    return DS;
                }
            }
        }
        #endregion

        #region Data Reader
        public IDataReader fn_DataReader(string procedure, params SqlParameter[] _Sqlparam)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {

                using (SqlCommand cmd = new SqlCommand(dbo_schema + "." + procedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter param in _Sqlparam)
                    {
                        cmd.Parameters.Add(param);
                    }
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(rdr);
                        return dt.CreateDataReader();
                    }
                }
            }
        }
        #endregion

        #region DataTable
        public DataTable fn_DataTable(string procedure)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlDataAdapter tda = new SqlDataAdapter(dbo_schema + "." + procedure, con))
                {
                    tda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable DT = new DataTable();
                    lock (lockObject)
                    {
                        tda.Fill(DT);
                    }

                    return DT;
                }
            }
        }
        #endregion

        #region DataTable
        public DataTable fn_DataTable(string procedure, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlDataAdapter tda = new SqlDataAdapter(dbo_schema + "." + procedure, con))
                {
                    tda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter param in sqlParameters)
                    {
                        tda.SelectCommand.Parameters.Add(param);
                    }
                    DataTable DT = new DataTable();
                    lock (lockObject)
                    {
                        tda.Fill(DT);
                    }

                    return DT;
                }
            }
        }
        #endregion

        #region Execute Non Query
        public int func_ExecuteNonQuery(string procedure, params SqlParameter[] _SqlParam)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlCommand cmd = new SqlCommand(dbo_schema + "." + procedure.ToString(), con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter para in _SqlParam)
                    {
                        cmd.Parameters.Add(para);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Execute Scalar
        public object func_ExecuteScalar(string procedure, params SqlParameter[] _SqlParam)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlCommand cmd = new SqlCommand(dbo_schema + "." + procedure.ToString(), con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter para in _SqlParam)
                    {
                        cmd.Parameters.Add(para);
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion

    }
}

