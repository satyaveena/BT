using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BT.TS360API.WebAPI.Models;
using BT.TS360API.WebAPI.Common;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.ServiceReferenceUserAlerts;
using BT.TS360API.WebAPI.Common.Helper;

namespace BT.TS360API.WebAPI.Services
{
    public class DistributedRepository
    {
        private const string CacheKey = "DistributedStore";

        public DistributedRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var distributedItems = new DistributedItem[] { };

                    ctx.Cache[CacheKey] = distributedItems;
                }
            }
        }

        public DistributedItem[] GetAllDistributedItems()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (DistributedItem[])ctx.Cache[CacheKey];
            }

            return new DistributedItem[] { };
        }

        public string SaveDist(DistributedItem distributedItem)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((DistributedItem[])ctx.Cache[CacheKey]).ToList();
                    currentData.Add(distributedItem);
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    string apiVersion = null;
                    if (distributedItem.Job != null)
                    {
                        apiVersion = distributedItem.Job.ApiVersion;

                        if (!string.IsNullOrEmpty(apiVersion) && apiVersion.Length >= 5)
                        {
                            apiVersion = apiVersion.Substring(0, 5);
                        }
                    }

                    // Update distribution to Orders repository
                    DataSet dsDistributionResponse = ConvertDistributedItemToSqlTable(distributedItem, apiVersion);

                    if (dsDistributionResponse.Tables.Count >= 2)
                    {
                        DataTable dtDistributedItem = dsDistributionResponse.Tables[0];
                        DataTable dtRankedItem = dsDistributionResponse.Tables[1];

                        OrdersDAO ordersDAO = new OrdersDAO();
                        ordersDAO.UpdateDistribution(distributedItem.CartId, dtDistributedItem, dtRankedItem, apiVersion);

                    }

                    return "";
                }
                catch (Exception ex)
                {
                    //return ex.Message;
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return "";
        }


        private DataSet ConvertDistributedItemToSqlTable(DistributedItem distributedItem, string apiVersion)
        {
            DataTable tblDistributedItem = new DataTable("BasketLineItemsDistributed");
            DataTable tblRankedItem = new DataTable("BasketLineItemsRanked");
            
            DataSet dsResult = new DataSet();
            
            // Distributed Items
            tblDistributedItem.Columns.Add("BasketLineItemId", typeof(String));
            //tblDistributedItem.Columns.Add("TotalQuantity", typeof(Int32));
            tblDistributedItem.Columns.Add("FundId", typeof(String));
            //tblDistributedItem.Columns.Add("FundCode", typeof(String));
            tblDistributedItem.Columns.Add("BranchId", typeof(String));
            //tblDistributedItem.Columns.Add("BranchCode", typeof(String));
            tblDistributedItem.Columns.Add("BranchQuantity", typeof(Int32));

            // Ranked Items
            tblRankedItem.Columns.Add("BasketLineItemId", typeof(String));
            tblRankedItem.Columns.Add("ESPOverallRanking", typeof(Decimal));
            tblRankedItem.Columns.Add("Value", typeof(Decimal));
            tblRankedItem.Columns.Add("ESPRankingName", typeof(String));
            tblRankedItem.Columns.Add("ESPRankingDescription", typeof(String));
            tblRankedItem.Columns.Add("Weight", typeof(Int32));
            tblRankedItem.Columns.Add("Confidence", typeof(Int32));
            tblRankedItem.Columns.Add("ESPBisacRanking", typeof(Decimal));
            tblRankedItem.Columns.Add("ESPBisacLiteral", typeof(string));
            tblRankedItem.Columns.Add("ESPDetailURL", typeof(string));
            tblRankedItem.Columns.Add("ESPCategory", typeof(string));
            tblRankedItem.Columns.Add("DetailHeight", typeof(Int32)); // ESP release 4.2
            tblRankedItem.Columns.Add("DetailWidth", typeof(Int32)); // ESP release 4.2

            int lineqty = 0;
            if (distributedItem.Items == null)
            {
                dsResult.Tables.Add(tblDistributedItem);
                dsResult.Tables.Add(tblRankedItem);
                return dsResult;
            }

            foreach (DistItem item in distributedItem.Items)
            {
                string basketLineItemID = item.LineItemId;
                int overallqty = item.Quantity;
                string fundid = item.FundId;
                string fundcode = item.FundCode;
                string branchid = "";
                string branchcode = "";
                int branchqty = 0;
                lineqty = 0;

                double? overall = null;
                double? bisacRanking = null;
                string bisacLiteral = null;
                string espDetailUrl = "";
                string overallScoreType = null;

                int? detailHeight = null;
                int? detailWidth = null;

                DistRanking ranking = item.Ranking;
                if (ranking != null)
                {
                    overall = ranking.Overall;
                    bisacRanking = ranking.Genre_Score;
                    bisacLiteral = ranking.Genre_Description;
                    espDetailUrl = ranking.Detail_Url;
                    overallScoreType = ranking.OverallScoreType;
                    detailHeight = ranking.DetailHeight;
                    detailWidth = ranking.DetailWidth;
                }

                DataRow rankedRow = tblRankedItem.NewRow();

                rankedRow["BasketLineItemId"] = basketLineItemID;
                
                if (overall.HasValue)
                {
                    rankedRow["ESPOverallRanking"] = overall;
                }

                if (bisacRanking.HasValue)
                {
                    rankedRow["ESPBisacRanking"] = bisacRanking;
                }

                if (!string.IsNullOrEmpty(bisacLiteral))
                {
                    rankedRow["ESPBisacLiteral"] = bisacLiteral;
                }

                rankedRow["ESPDetailURL"] = espDetailUrl;

                rankedRow["ESPCategory"] = ESPHelper.GetCategoryType(apiVersion, overallScoreType);

                if (detailHeight.HasValue)
                {
                    rankedRow["DetailHeight"] = detailHeight;
                }

                if (detailWidth.HasValue)
                {
                    rankedRow["DetailWidth"] = detailWidth;
                }

                tblRankedItem.Rows.Add(rankedRow);

                if (item.Grid == null || item.Grid.Length == 0)
                {
                    DataRow workRow = tblDistributedItem.NewRow();
                    workRow["BasketLineItemId"] = basketLineItemID;
                    //workRow["TotalQuantity"] = overallqty; // this is causing error when gird is not provided
                    workRow["FundId"] = fundid;
                    //workRow["FundCode"] = fundcode;

                    tblDistributedItem.Rows.Add(workRow);

                    continue;
                }

                foreach (Grid grid in item.Grid)
                {

                    branchid = grid.BranchId;
                    //branchcode = grid.BranchCode;
                    branchqty = grid.Quantity;

                    DataRow workRow = tblDistributedItem.NewRow();
                    workRow["BasketLineItemId"] = basketLineItemID;
                    //workRow["TotalQuantity"] = overallqty;
                    workRow["FundId"] = fundid;
                    //workRow["FundCode"] = fundcode;
                    workRow["BranchId"] = branchid;
                    //workRow["BranchCode"] = branchcode;
                    workRow["BranchQuantity"] = branchqty;

                    tblDistributedItem.Rows.Add(workRow);
                    lineqty = lineqty + branchqty;
                }
                if (overallqty != lineqty)
                {
                    throw new Exception("distributed qty <> branch qty", null);
                }

            }

            //return tblDistributedItem;
            dsResult.Tables.Add(tblDistributedItem);
            dsResult.Tables.Add(tblRankedItem);
            return dsResult;
        }

        public string SendAlert(DistributedItem distributedItem, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                string alertMessageTemplate = string.Empty;
                //string AlertMessageComplete = string.Empty;
                string configReferenceValue = string.Empty;

                OrdersDAO ordersDAO = new OrdersDAO();

                // Get the user id
                ESPCartState espDistState = ordersDAO.GetBasketByID(distributedItem.CartId,"DIST");

                // Create the alert
                UserAlertsClient userAlert = new UserAlertsClient(); 

                
                GetUserAlertMessageTemplateResponse svc1Getresp = new GetUserAlertMessageTemplateResponse();

                if (espDistState.DistType == "DIST-WFunding") 
                   { svc1Getresp = userAlert.GetUserAlertMessageTemplate(AlertMessageTemplateIDEnum.ESPDistWFundComplete); }
                else
                   { svc1Getresp = userAlert.GetUserAlertMessageTemplate(AlertMessageTemplateIDEnum.ESPDistWOFundComplete); }



                if (svc1Getresp.Status == "OK")
                {
                    alertMessageTemplate = svc1Getresp.AlertMessageTemplate;
                    configReferenceValue = svc1Getresp.ConfigReferenceValue;
                }
                else
                {
                    errorMessage = "GetUserAlert: " + svc1Getresp.ErrorMessage; 
                    throw new Exception("GetUserAlert: " + svc1Getresp.ErrorMessage);
                }



                alertMessageTemplate = alertMessageTemplate.Replace("@cartname", espDistState.CartName);
                alertMessageTemplate = alertMessageTemplate.Replace("@URL", configReferenceValue + distributedItem.CartId);

                CreateUserAlertMessageResponse svc1resp = new CreateUserAlertMessageResponse();

                if (espDistState.DistType == "DIST-WFunding")
                { svc1resp = userAlert.CreateUserAlertMessage(alertMessageTemplate, espDistState.UserID, AlertMessageTemplateIDEnum.ESPDistWFundComplete, "WebAPI"); }
                else
                { svc1resp = userAlert.CreateUserAlertMessage(alertMessageTemplate, espDistState.UserID, AlertMessageTemplateIDEnum.ESPDistWOFundComplete, "WebAPI"); }


                if (svc1resp.Status != "OK")
                {
                     
                    errorMessage = "WriteUserAlert: " + svc1resp.ErrorMessage; 
                    throw new Exception("WriteUserAlert: " + svc1resp.ErrorMessage);
                }


            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                throw new Exception(ex.Message); 
            }

            return "";

        }
    }
}