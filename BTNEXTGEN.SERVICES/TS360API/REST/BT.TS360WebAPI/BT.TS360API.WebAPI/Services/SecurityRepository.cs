using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Configuration;
using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.DataAccess; 


namespace BT.TS360API.WebAPI.Services
{
    public class SecurityRepository
    {
        public SecurityRepository() { }

        public bool ValidateAPIKey(HttpRequestMessage request, out string apiKeyHeaderValue)
        {
            bool isValid = false;
            apiKeyHeaderValue = string.Empty;

            HttpRequestHeaders headers = request.Headers;

            IEnumerable<string> apiKeyHeaderValues = null;

            if (headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
            {
                apiKeyHeaderValue = apiKeyHeaderValues.First();
                isValid = (apiKeyHeaderValue == AppSetting.VendorAPIKey) ? true : false;
            }


            return isValid;

        }

        public bool ValidateAPIKeyDB(HttpRequestMessage request, out string apiKeyHeaderValue)
        {
            bool isValid = false;
            apiKeyHeaderValue = string.Empty; 

                        HttpRequestHeaders headers = request.Headers;

            IEnumerable<string> apiKeyHeaderValues = null;

            if (headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
            {

                apiKeyHeaderValue = apiKeyHeaderValues.First();

                if (ValidateAPIKeyDBCall(apiKeyHeaderValue) == true)
                { isValid = true;  }


            }


            return isValid;

        }

        public static bool ValidateAPIKeyDBCall(string APIKey)
        {
            bool ValidKey;
            ValidKey = false; 

            var dataConnect = ConfigurationManager.ConnectionStrings["Profiles"].ConnectionString;

            SqlConnection ProfilesDBConnection = new SqlConnection(dataConnect);

            SqlDataReader dataReader = null;

            try
            {
                SqlCommand storedProc = new SqlCommand("procTS360APIValidateKey", ProfilesDBConnection);

                storedProc.CommandType = CommandType.StoredProcedure;

                SqlParameter APIKeyIn = storedProc.Parameters.Add
                  ("@APIKey", SqlDbType.VarChar, 32);
                APIKeyIn.Direction = ParameterDirection.Input;

                APIKeyIn.Value = APIKey;
                
                ProfilesDBConnection.Open();

                dataReader = storedProc.ExecuteReader();

                while (dataReader.Read())
                {
                    ValidKey = dataReader.GetBoolean(0); 

                };

            }
            catch (Exception ex)
            {
                // log exception and exit gracefully 
                CreateRepository.LogAPIMessage("ValidateAPIKeyDBCall", "", "", "", ex.Message);
                
            }
            finally
            {
                dataReader.Close();
                ProfilesDBConnection.Close();
            }

            return ValidKey;
        }


    }
}