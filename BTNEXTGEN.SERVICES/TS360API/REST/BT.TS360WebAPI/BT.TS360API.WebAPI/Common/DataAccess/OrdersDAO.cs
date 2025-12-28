using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Serialization;

using BT.TS360API.WebAPI.Common.Constants;
using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.Helper;
using BT.TS360API.WebAPI.Models;


namespace BT.TS360API.WebAPI.Common.DataAccess
{
    public class OrdersDAO
    {
        private SqlConnection _sqlConn;

        public OrdersDAO()
        {
            _sqlConn = new SqlConnection(AppSetting.OrdersDatabaseConnectionString);
        }

        public void UpdateRank(string libraryID, string basketSummaryID, DataTable basketLineItemsRanking, string apiVersion)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@ESPLibraryId", libraryID));
            alParams.Add(new SqlParameter("@BasketSummaryId", basketSummaryID));
            alParams.Add(new SqlParameter("@BasketLineItemsRanking", SqlDbType.Structured) { Value = basketLineItemsRanking });
            alParams.Add(new SqlParameter("@APIVersion", apiVersion));

            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DatabaseHelper.ExecuteNonQuery(Constants.StoredProcedure.UPDATE_CART_RANK, arr, _sqlConn, out sqlError);

            if (!string.IsNullOrEmpty(sqlError))
            {
                throw new Exception(sqlError);
            }

        }

        public ESPCartState GetBasketByID(string basketSummaryID, string ESPJobType)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@BasketSummaryId", basketSummaryID));
            alParams.Add(new SqlParameter("@ESPJobType", ESPJobType));
            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.GET_ESP_STATE, arr, _sqlConn, out sqlError);

            ESPCartState espRankState = new ESPCartState();

            if (!string.IsNullOrEmpty(sqlError))
                throw new Exception(sqlError);
            
            if (result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                throw new Exception(StoredProcedure.GET_ESP_STATE + ": Invalid Basket");

            espRankState = new ESPCartState
            {
                UserID = result.Tables[0].Rows[0]["UserId"].ToString(),
                CartId = basketSummaryID,
                CartName = result.Tables[0].Rows[0]["BasketName"].ToString(), 
                DistType = result.Tables[0].Rows[0]["DistType"].ToString()

            };

            return espRankState;

        }

        public void UpdateDistribution(string basketSummaryID, DataTable basketLineItemsDistribution, DataTable basketLineItemsRanking, string apiVersion)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@BasketSummaryID", basketSummaryID));
            alParams.Add(new SqlParameter("@udtESPBasketGridLines", SqlDbType.Structured) { Value = basketLineItemsDistribution });
            alParams.Add(new SqlParameter("@udtBasketLineItemsRanking", SqlDbType.Structured) { Value = basketLineItemsRanking });
            alParams.Add(new SqlParameter("@APIVersion", apiVersion));

            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DatabaseHelper.ExecuteNonQuery(Constants.StoredProcedure.UPDATE_CART_DIST, arr, _sqlConn, out sqlError);

            if (!string.IsNullOrEmpty(sqlError))
            {
                throw new Exception(sqlError);
            }

        }

        public string ProcessESPSubmitRequest(string basketSummaryID, string userID, string espJobType, bool cartWizardOption)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@BasketSummaryId", basketSummaryID));
            alParams.Add(new SqlParameter("@Literal", espJobType));
            alParams.Add(new SqlParameter("@UserId", userID));

            if (espJobType == RankConstant.ESPType.DIST)
            {
                var paramCartWizardOption = new SqlParameter("@DefaultESPCartWizard", SqlDbType.Bit) { Direction = ParameterDirection.Input, Value = cartWizardOption };
                alParams.Add(paramCartWizardOption);
            }

            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DatabaseHelper.ExecuteNonQuery(Constants.StoredProcedure.SET_BASKET_SUMMARY_ESP_STATE, arr, _sqlConn, out sqlError);

            return sqlError;
        }

        public ESPRankDataRootRequest GetESPRankData(string basketSummaryID)
        {
            ESPRankDataRootRequest resultingMessage = new ESPRankDataRootRequest();

            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@BasketSummaryId", basketSummaryID));

            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.GET_RANK_REQUESTS, arr, _sqlConn, out sqlError);

            if (result.Tables.Count > 0 || result.Tables[0].Rows.Count > 0)
            {
                string xml = result.Tables[0].Rows[0]["NewRankRequests"].ToString();

                XmlSerializer serializer = new XmlSerializer(typeof(ESPRankDataRootRequest));
                StringReader rdr = new StringReader(xml);
                resultingMessage = (ESPRankDataRootRequest)serializer.Deserialize(rdr);

            }
                
            if (!string.IsNullOrEmpty(sqlError))
                throw new Exception( "GetESPRankData: " + sqlError);

            return resultingMessage;
        }

        public ESPDistDataRootRequest GetESPDistributionData(string basketSummaryID)
        {
            ESPDistDataRootRequest resultingMessage = new ESPDistDataRootRequest();

            ArrayList alParams = new ArrayList();

            alParams.Add(new SqlParameter("@BasketSummaryId", basketSummaryID));

            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.GET_DISTRIBUTION_REQUESTS, arr, _sqlConn, out sqlError);

            if (result.Tables.Count > 0 || result.Tables[0].Rows.Count > 0)
            {
                string xml = result.Tables[0].Rows[0]["NewDistributionRequests"].ToString();

                XmlSerializer serializer = new XmlSerializer(typeof(ESPDistDataRootRequest));
                StringReader rdr = new StringReader(xml);
                resultingMessage = (ESPDistDataRootRequest)serializer.Deserialize(rdr);

            }

            if (!string.IsNullOrEmpty(sqlError))
                throw new Exception("GetESPDistributionData: " + sqlError);

            return resultingMessage;
        }

        public void SetESPState(DataTable dtESPStatus, string jobUrl, string jobUrlText)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@jobURL", jobUrl));
            alParams.Add(new SqlParameter("@jobText", jobUrlText));
            
            alParams.Add(new SqlParameter("@ESPStatus", SqlDbType.Structured) { Value = dtESPStatus });
            
            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DatabaseHelper.ExecuteNonQuery(Constants.StoredProcedure.ESP_SET_STATE, arr, _sqlConn, out sqlError);

            if (!string.IsNullOrEmpty(sqlError))
            {
                throw new Exception("SetESPState: " + sqlError);
            }
        }

        public void SetESPState(DataSet dtESPResponse, string jobUrl, string jobUrlText)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@jobURL", jobUrl));
            alParams.Add(new SqlParameter("@jobText", jobUrlText));

            if (dtESPResponse != null)
            {
                if (dtESPResponse.Tables.Count >= 1)
                {
                    alParams.Add(new SqlParameter("@ESPStatus", SqlDbType.Structured) { Value = dtESPResponse.Tables[0] });
                }

                if (dtESPResponse.Tables.Count >= 2)
                {
                    alParams.Add(new SqlParameter("@InvalidBranchCodes", SqlDbType.Structured) { Value = dtESPResponse.Tables[1] });
                }

                if (dtESPResponse.Tables.Count >= 3)
                {
                    alParams.Add(new SqlParameter("@InvalidFundCodes", SqlDbType.Structured) { Value = dtESPResponse.Tables[2] });
                }
            }
            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DatabaseHelper.ExecuteNonQuery(Constants.StoredProcedure.ESP_SET_STATE, arr, _sqlConn, out sqlError);

            if (!string.IsNullOrEmpty(sqlError))
            {
                throw new Exception("SetESPState: " + sqlError);
            }
        }

    }

}