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
    public class RankedRepository
    {
        private const string CacheKey = "RankedStore";

        public RankedRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var rankedItems = new RankedItem[]{};

                    ctx.Cache[CacheKey] = rankedItems;
                }
            }
        }

        public RankedItem[] GetAllRankedItems()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (RankedItem[])ctx.Cache[CacheKey];
            }

            return new RankedItem[] {};
        }

        public string SaveRank(RankedItem rankedItem)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((RankedItem[])ctx.Cache[CacheKey]).ToList();
                    currentData.Add(rankedItem);
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    string apiVersion = null;
                    if (rankedItem.Job != null)
                    {
                        apiVersion = rankedItem.Job.ApiVersion;

                        if (!string.IsNullOrEmpty(apiVersion) && apiVersion.Length >= 5)
                        {
                            apiVersion = apiVersion.Substring(0, 5);
                        }
                    }

                    // Update rank to Orders repository
                    DataTable dtRankedItem = ConvertRankedItemToSqlTable(rankedItem, apiVersion);

                    OrdersDAO ordersDAO = new OrdersDAO();
                    ordersDAO.UpdateRank(rankedItem.ESPLibraryId, rankedItem.CartId, dtRankedItem, apiVersion);

                    return "";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            return "";
        }


        private DataTable ConvertRankedItemToSqlTable(RankedItem rankedItem, string apiVersion)
        {
            DataTable tblRankedItem = new DataTable("BasketLineItemsRanking");

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

            if (rankedItem.Items == null) return tblRankedItem;

            foreach (Item item in rankedItem.Items)
            {
                string basketLineItemID = item.LineItemId;
                double? overall = null;
                string espRankingName = "";
                string espRankingDescription = "";
                int weight = 0;
                int confidence = 0;
                double value = 0;

                double? bisacRanking = null;
                string bisacLiteral = null;
                string espDetailUrl = "";
                string overallScoreType = null;

                int? detailHeight = null;
                int? detailWidth = null;

                Ranking ranking = item.Ranking;
                if (ranking == null) continue;

                overall = ranking.Overall;
                bisacRanking = ranking.Genre_Score;
                bisacLiteral = ranking.Genre_Description;
                espDetailUrl = ranking.Detail_Url;
                overallScoreType = ranking.OverallScoreType;
                detailHeight = ranking.DetailHeight;
                detailWidth = ranking.DetailWidth;

                if (ranking.Detail == null || ranking.Detail.Length == 0) 
                {
                    DataRow workRow = tblRankedItem.NewRow();
                    workRow["BasketLineItemId"] = basketLineItemID;
                    
                    if (overall.HasValue)
                    { 
                        workRow["ESPOverallRanking"] = overall;
                    }

                    if (bisacRanking.HasValue)
                    { 
                        workRow["ESPBisacRanking"] = bisacRanking;
                    }

                    if (!string.IsNullOrEmpty(bisacLiteral))
                    { 
                        workRow["ESPBisacLiteral"] = bisacLiteral;
                    }

                    workRow["ESPDetailURL"] = espDetailUrl;

                    workRow["ESPCategory"] = ESPHelper.GetCategoryType(apiVersion, overallScoreType);

                    if (detailHeight.HasValue)
                    { 
                        workRow["DetailHeight"] = detailHeight;
                    }

                    if (detailWidth.HasValue)
                    { 
                        workRow["DetailWidth"] = detailWidth;
                    }

                    tblRankedItem.Rows.Add(workRow);

                    continue; 
                }

                foreach (Detail detail in ranking.Detail)
                {
                    espRankingName = detail.Name;
                    espRankingDescription = detail.Description;
                    weight = detail.Weight;
                    confidence = detail.Confidence;
                    value = detail.Value;

                    DataRow workRow = tblRankedItem.NewRow();
                    workRow["BasketLineItemId"] = basketLineItemID;

                    if (overall.HasValue)
                    { 
                        workRow["ESPOverallRanking"] = overall;
                    }

                    workRow["Value"] = value;
                    workRow["ESPRankingName"] = espRankingName;
                    workRow["ESPRankingDescription"] = espRankingDescription;
                    workRow["Weight"] = weight;
                    workRow["Confidence"] = confidence;

                    if (bisacRanking.HasValue)
                    { 
                        workRow["ESPBisacRanking"] = bisacRanking;
                    }

                    if (!string.IsNullOrEmpty(bisacLiteral))
                    {
                        workRow["ESPBisacLiteral"] = bisacLiteral;
                    }

                    workRow["ESPDetailURL"] = espDetailUrl;

                    workRow["ESPCategory"] = ESPHelper.GetCategoryType(apiVersion, overallScoreType);

                    if (detailHeight.HasValue)
                    {
                        workRow["DetailHeight"] = detailHeight;
                    }

                    if (detailWidth.HasValue)
                    {
                        workRow["DetailWidth"] = detailWidth;
                    }

                    tblRankedItem.Rows.Add(workRow);
                }
                
            }

            return tblRankedItem;
        }

        public bool SendAlert(RankedItem rankedItem, out string errorMessage)
        {
            bool isSent = false;
            errorMessage = string.Empty;
            try
            {

                OrdersDAO ordersDAO = new OrdersDAO();

                // Get the user id
                ESPCartState espRankState = ordersDAO.GetBasketByID(rankedItem.CartId, "RANK");

                // Create the alert
                UserAlertsClient userAlert = new UserAlertsClient();

                GetUserAlertMessageTemplateResponse svc1Getresp = new GetUserAlertMessageTemplateResponse();
                svc1Getresp = userAlert.GetUserAlertMessageTemplate(AlertMessageTemplateIDEnum.ESPRankComplete);

                string alertMessage = svc1Getresp.AlertMessageTemplate;
                string configReferenceValue = svc1Getresp.ConfigReferenceValue;

                //string alertMessageTemplate = "The ESP Function – Ranking has completed for cart <b>@cartname</b>. <a href = “@URL”><b>REVIEW CART NOW</b></a>";
                alertMessage = alertMessage.Replace("@cartname", espRankState.CartName);
                alertMessage = alertMessage.Replace("@URL", configReferenceValue + rankedItem.CartId);

                CreateUserAlertMessageResponse svc1resp = new CreateUserAlertMessageResponse();

                svc1resp = userAlert.CreateUserAlertMessage(alertMessage, espRankState.UserID, AlertMessageTemplateIDEnum.ESPRankComplete, "WebAPI");

                isSent = (svc1resp.Status == "OK");

                if (!string.IsNullOrEmpty(svc1resp.ErrorMessage))
                    errorMessage = svc1resp.ErrorMessage;

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isSent = false;
            }

            return isSent;
        }
    }
}