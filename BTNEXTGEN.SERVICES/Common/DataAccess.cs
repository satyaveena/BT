using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BTNextGen.Services.Common
{
    public class DataAccess
    {
        private SqlConnection sqlConn = null;

        public void OpenConnection(string connectionString)
        {
            sqlConn = new SqlConnection();
            sqlConn.ConnectionString = connectionString;
            sqlConn.Open();
        }

        public void CloseConnection(string connectionString)
        {
            sqlConn.Close();
        }

        public DataTable GetOrderDetails(string accountNum, string conn)
        {
            try
            {
                OpenConnection(conn);

                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procGetBasketFolderByName]";// the stored procedure and the input values used are only for test purpose.

                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@BasketFolderName";
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Value = "Ordered & Submitted Carts";
                    sqlCmd.Parameters.Add(param);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@UserId";
                    param1.SqlDbType = SqlDbType.VarChar;
                    param1.Value = "{00000000-0000-0000-0000-000000000000}";
                    sqlCmd.Parameters.Add(param1);


                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    CloseConnection(conn);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }

        }


        //public void InsertTVPRecords(DataTable dtBTKeySeq, string conn)
        //{
        //    try
        //    {
        //        OpenConnection(conn);

        //        string storedProcName = "[dbo].[InsertBTKeySequenceTVP]";// the stored procedure and the input values used are only for test purpose.

        //        using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
        //        {
        //            sqlCmd.CommandType = CommandType.StoredProcedure;
        //            // Input Paramter

        //            SqlParameter tvpparam = sqlCmd.Parameters.AddWithValue("@BTKeySeqTVP", dtBTKeySeq);
        //            tvpparam.SqlDbType = SqlDbType.Structured;
        //            sqlCmd.ExecuteNonQuery();
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        CloseConnection(conn);
        //    }
        //}


   


    }
}
