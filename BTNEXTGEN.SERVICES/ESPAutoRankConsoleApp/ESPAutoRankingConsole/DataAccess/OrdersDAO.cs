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

using ESPAutoRankingConsole.Common;
using ESPAutoRankingConsole.Common.Helpers;
using System.Xml;
using System.Data.SqlTypes;
using System.Xml.Linq;
using Elmah;
using ESPAutoRankingConsole.DataModels;

namespace ESPAutoRankingConsole.DataAccess
{
    public class OrdersDAO
    {
        private SqlConnection _sqlConn;
        private const string BasketLineItemIDsTableType = "[dbo].[utblBasketLineItemIDs]";

        public OrdersDAO()
        {
            _sqlConn = new SqlConnection(AppSettings.OrdersConnectionString);
        }

        public void SetESPAutoRankStatus(string cartId, List<ESPRankItemJsonRequest> items, string userId, ESPAutoRankQueueStatus status)
        {
            var lineItemIDsParam = ConvertToLineItemIdArgumentTable(items, CreateLineItemIdArgumentTable());

            var alParams = new ArrayList();
            alParams.Add(new SqlParameter("@BasketSummaryID", cartId));
            alParams.Add(new SqlParameter("@BasketLineItemsIds", SqlDbType.Structured) 
            {
                TypeName = BasketLineItemIDsTableType,
                Value = lineItemIDsParam
            });

            alParams.Add(new SqlParameter("@UserID", userId));
            alParams.Add(new SqlParameter("@Status", status.ToString()));

            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));

            string errorMessage;
            DatabaseHelper.ExecuteNonQuery(StoredProcedure.TS360_SET_ESP_AUTO_RANK_QUEUE_STATUS, arr, _sqlConn, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
                throw new Exception(errorMessage);
        }

        public XmlDocument ESPGetAutoRankRequests(int basketsMax, string threadId)
        {
            
            XmlDocument espAutoRankRequests = new XmlDocument();
            try
            {
                SqlCommand cmd = new SqlCommand(StoredProcedure.ESP_GET_AUTO_RANK_REQUESTS, _sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MaxBasketCount", basketsMax));
                cmd.Parameters.Add(new SqlParameter("@ThreadID", threadId));
                SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(paramErrorMessage);

                _sqlConn.Open();

                using (var reader = cmd.ExecuteXmlReader())
                {
                    if (reader.Read())
                        espAutoRankRequests.Load(reader);
                }

                object paramValue = cmd.Parameters["@ErrorMessage"].Value;
                var errorMessage = paramValue != null ? paramValue.ToString() : "";

                if (!string.IsNullOrEmpty(errorMessage))
                    throw new Exception(errorMessage);
            }
            finally
            {
                _sqlConn.Close();
            }
            return espAutoRankRequests; 
        }

        private DataTable CreateLineItemIdArgumentTable()
        {
            var dt = new DataTable("utblBasketLineItemIDs");
            dt.Columns.Add("BasketLineItemID", typeof(string));
            return dt;
        }

        private DataTable ConvertToLineItemIdArgumentTable(List<ESPRankItemJsonRequest> items, DataTable dt)
        {
            if (items != null && items.Any())
            {
                items.ForEach(r => dt.Rows.Add(r.lineItemId));
            }
            return dt;
        }

    }

}