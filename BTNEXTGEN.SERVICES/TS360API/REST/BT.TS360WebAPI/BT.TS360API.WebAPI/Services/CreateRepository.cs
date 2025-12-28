using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;
using System.ServiceModel;
using BT.TS360API.WebAPI.Common.Configuration;


namespace BT.TS360API.WebAPI.Services
{
    public class CreateRepository
    {
        //private string dbConnectionString;
        //private SqlConnection dbConnection;

        public enum ExpectedType
        { 
            StringType = 0,
            NumberType=1,
            DateType = 2,
            BooleanType = 3,
            ImageType = 4
        }

        public CreateRepository()
        {
        }

        public static string ValidateUserDAL(string UserName, string Email)
        {
            var userId = string.Empty;

            var dataConnect = ConfigurationManager.ConnectionStrings["SSO"].ConnectionString;

            SqlConnection SSODBConnection = new SqlConnection(dataConnect);

            SqlDataReader dataReader = null;

            try
            {
                SqlCommand storedProc = new SqlCommand("procTS360APIValidateUser", SSODBConnection);

                storedProc.CommandType = CommandType.StoredProcedure;

                SqlParameter UserNameIn = storedProc.Parameters.Add
                  ("@UserName", SqlDbType.VarChar, 50);
                UserNameIn.Direction = ParameterDirection.Input;

                SqlParameter EmailIn = storedProc.Parameters.Add
                   ("@Email", SqlDbType.VarChar, 64);
                EmailIn.Direction = ParameterDirection.Input;

                UserNameIn.Value = UserName;
                EmailIn.Value = Email;

                SSODBConnection.Open();

                dataReader = storedProc.ExecuteReader();

                while (dataReader.Read())
                {
                    userId = dataReader.GetString(0);
                };

            }
            catch (Exception ex)
            {
                // log exception and exit gracefully 
                LogAPIMessage("DAL.ValidateUserDAL", "", "", "", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
                
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
               
                SSODBConnection.Close();
            }

            return userId;
        }


        public static void LogAPIMessage(
            string webMethod, string requestMessage,
            string responseMessage, string vendorAPIKey,
            string exceptionMessage 
            )
        {
            var dataConnect = ConfigurationManager.ConnectionStrings["ExceptionLogging"].ConnectionString;

            SqlConnection DBConnection = new SqlConnection(dataConnect);

            try
            {
                SqlCommand storedProc = new SqlCommand("procTS360APILogRequests", DBConnection);

                storedProc.CommandType = CommandType.StoredProcedure;


                SqlParameter webMethodIn = storedProc.Parameters.Add("@webMethod", SqlDbType.NVarChar, 50);
                webMethodIn.Direction = ParameterDirection.Input;
                webMethodIn.Value = webMethod;

                SqlParameter requestMessageIn = storedProc.Parameters.Add("@requestMessage", SqlDbType.NVarChar, 8000);
                requestMessageIn.Direction = ParameterDirection.Input;
                requestMessageIn.Value = requestMessage;

                SqlParameter responseMessageIn = storedProc.Parameters.Add("@responseMessage", SqlDbType.NVarChar, 8000);
                responseMessageIn.Direction = ParameterDirection.Input;
                responseMessageIn.Value = responseMessage;

                SqlParameter vendorAPIKeyIn = storedProc.Parameters.Add("@vendorAPIKey", SqlDbType.NVarChar, 255);
                vendorAPIKeyIn.Direction = ParameterDirection.Input;
                vendorAPIKeyIn.Value = vendorAPIKey;

                SqlParameter exceptionMessageIn = storedProc.Parameters.Add("@exceptionMessage", SqlDbType.NVarChar, 8000);
                exceptionMessageIn.Direction = ParameterDirection.Input;
                exceptionMessageIn.Value = exceptionMessage;

                SqlParameter createdOnIn = storedProc.Parameters.Add("@createdOn", SqlDbType.DateTime2, 7);
                createdOnIn.Direction = ParameterDirection.Input;
                createdOnIn.Value = DateTime.Now;

                SqlParameter createdByIn = storedProc.Parameters.Add("@createdBy", SqlDbType.NVarChar, 50);
                createdByIn.Direction = ParameterDirection.Input;
                createdByIn.Value = "TS360 API Service";

                DBConnection.Open();

                var records = storedProc.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
               
                DBConnection.Close();
            }

        }

        public static DataTable GetISBNListDAL(DataTable btkeyList)
        {
            DataTable ISBNList = new DataTable();

            var dataConnect = ConfigurationManager.ConnectionStrings["ProductCatalog"].ConnectionString;

            SqlConnection ProductCatalogDBConnection = new SqlConnection(dataConnect);

            try
            {

                int SQLCommandTimeout;
                SQLCommandTimeout = Convert.ToInt32(AppSetting.CreateSQLCommandTimeout);

                // Stored Proc. name
                SqlCommand storedProc = new SqlCommand("procTS360APIGetISBNsByBTKeys", ProductCatalogDBConnection);

                storedProc.CommandType = CommandType.StoredProcedure;

                // Note the UDT parameter
                storedProc.Parameters.Add(new SqlParameter("@BTKeyList", SqlDbType.Structured));
                storedProc.Parameters["@BTKeyList"].Value = btkeyList;
                storedProc.CommandTimeout = SQLCommandTimeout; 

                ProductCatalogDBConnection.Open();

                SqlDataAdapter sda = new SqlDataAdapter(storedProc);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                

                // Retrieving total stored tables from common DataSet            
                ISBNList = ds.Tables[0];

                // To display all rows of a table, we use foreach loop for each DataTable.
                //foreach (DataRow dr in dt1.Rows)
                //{
                //    Console.WriteLine("Student Name: " + dr[sName]);
                //}

            }
            catch (Exception ex)
            {
                // log exception and exit gracefully 
                LogAPIMessage("DAL.GetISBNListDAL", "", "", "", ex.Message);
            }
            finally
            {
                ProductCatalogDBConnection.Close();
            }

            return ISBNList;
        }

        public static DataTable CreateCart(
            DataTable utblListTransferBasketsType, 
            DataTable utblListTransferBasketDetailsType, out string basketGuid, out string dbReturn, out string dbMessage)
        {
            DataTable TS360Basket = new DataTable();

            var dataConnect = ConfigurationManager.ConnectionStrings["Orders"].ConnectionString;

            SqlConnection OrdersDBConnection = new SqlConnection(dataConnect);

            var  basketGUID = string.Empty;
            dbReturn = string.Empty;
            dbMessage = string.Empty; 

            try
            {

                int SQLCommandTimeout;
                SQLCommandTimeout = Convert.ToInt32(AppSetting.CreateSQLCommandTimeout);

                // Stored Proc. name
                SqlCommand storedProc = new SqlCommand("procListTransferInsertBasketsAPI", OrdersDBConnection);

                storedProc.CommandType = CommandType.StoredProcedure;

                // Note the UDT parameter
                storedProc.Parameters.Add(new SqlParameter("@parBasket", SqlDbType.Structured));
                storedProc.Parameters["@parBasket"].Value = utblListTransferBasketsType;

                storedProc.Parameters.Add(new SqlParameter("@parBasketDetails", SqlDbType.Structured));
                storedProc.Parameters["@parBasketDetails"].Value = utblListTransferBasketDetailsType;


                SqlParameter returnOutput = new SqlParameter("@Return", SqlDbType.Int);
                returnOutput.Direction = ParameterDirection.Output;
                SqlParameter messageOutput = new SqlParameter("@Message", SqlDbType.NVarChar, 8000);
                messageOutput.Direction = ParameterDirection.Output;
                SqlParameter basketLineCountOutput = new SqlParameter("@BasketLineCount", SqlDbType.Int);
                basketLineCountOutput.Direction = ParameterDirection.Output;
                SqlParameter basketlinecountINSERTEDOutput = new SqlParameter("@basketlinecountINSERTED", SqlDbType.Int);
                basketlinecountINSERTEDOutput.Direction = ParameterDirection.Output;
                SqlParameter basketGUIDOutput = new SqlParameter("@basketGUID", SqlDbType.NVarChar, 50);
                basketGUIDOutput.Direction = ParameterDirection.Output;

                // read the recordset into a datareader to determine invalid vs restricted // loaderror & btkey

                storedProc.Parameters.Add(returnOutput);
                storedProc.Parameters.Add(messageOutput);
                storedProc.Parameters.Add(basketLineCountOutput);
                storedProc.Parameters.Add(basketlinecountINSERTEDOutput);
                storedProc.Parameters.Add(basketGUIDOutput);
                storedProc.CommandTimeout = SQLCommandTimeout; 

             //@Return int output, 
             //@Message nvarchar(max) OUTPUT,
             //@BasketLineCount int output,
             //@basketlinecountINSERTED int output,
             //@basketGUID nvarchar(50) OUTPUT 

                OrdersDBConnection.Open();
                //storedProc.ExecuteNonQuery();

                

                SqlDataAdapter sda = new SqlDataAdapter(storedProc);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                basketGUID = basketGUIDOutput.Value.ToString();
                dbReturn = returnOutput.Value.ToString();
                dbMessage = messageOutput.Value.ToString();

                // Retrieving total stored tables from common DataSet      
                if (ds.Tables.Count > 0)
                {
                    TS360Basket = ds.Tables[0];
                }

                // To display all rows of a table, we use foreach loop for each DataTable.
                //foreach (DataRow dr in dt1.Rows)
                //{
                //    Console.WriteLine("Student Name: " + dr[sName]);
                //}

            }
            catch (Exception ex)
            {
                // log exception and exit gracefully 
                LogAPIMessage("DAL.CreateCart", "", "", "",ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                OrdersDBConnection.Close();
            }

            basketGuid = basketGUID;

            return TS360Basket;

        }
    
    }
}