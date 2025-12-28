using System.Collections.Generic;
using System.Linq;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Search;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;
using System;

namespace BT.TS360SP
{
    public class NRCFeaturedTitles : CMListBase<NRCFeaturedTitlesItem>
    {
        string RequestDomainName { get; set; }
        string Month { get; set; }
        string Year { get; set; }
        string ProductType { get; set; }
        public NRCFeaturedTitles(string month, string year, string productType, string requestDomainName)
        {
            Month = month;
            Year = year;
            ProductType = productType;
            RequestDomainName = requestDomainName;
            HasAdName = false;
            HasIsDefault = false;
            RowLimit = 30;
        }
        protected override string GetListName()
        {
            return CMListNameConstants.NRCFeaturedTitles;
        }

        protected override string AddWhereCamlTo(string condition)
        {
            string result = condition;
            //Get product type
            var productTypesList = new List<string>();
            productTypesList.Add(ProductType);

            result = result + ContentManagementHelper.BuildCamlString(productTypesList, CMFieldNameConstants.ProductType);

            DateTime startDate = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), 1);//the first date
            string startDateString = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

            DateTime endDate = startDate.AddMonths(1).AddDays(-1);//the last date
            string endDateString = endDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

            //release date
            string startReleaseDate = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime' IncludeTimeValue='FALSE'>{2}</Value>{3}", CMConstants.GEQ_TAG_OPEN, CMFieldNameConstants.ReleaseDate, startDateString, CMConstants.GEQ_TAG_CLOSE);
            string endReleaseDate = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime' IncludeTimeValue='FALSE'>{2}</Value>{3}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.ReleaseDate, endDateString, CMConstants.LEQ_TAG_CLOSE);

            //pre order date
            string startPreOrderDate = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime' IncludeTimeValue='FALSE'>{2}</Value>{3}", CMConstants.GEQ_TAG_OPEN, CMFieldNameConstants.PreOrderDate, startDateString, CMConstants.GEQ_TAG_CLOSE);
            string endPreOrderDate = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime' IncludeTimeValue='FALSE'>{2}</Value>{3}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.PreOrderDate, endDateString, CMConstants.LEQ_TAG_CLOSE);
                        
            string filterDate = CMConstants.OR_TAG_OPEN +
                                    CMConstants.AND_TAG_OPEN +
                                        startReleaseDate + endReleaseDate +
                                    CMConstants.AND_TAG_CLOSE +
                                    CMConstants.AND_TAG_OPEN +
                                        startPreOrderDate + endPreOrderDate +
                                    CMConstants.AND_TAG_CLOSE +
                                CMConstants.OR_TAG_CLOSE;

            result = CMConstants.AND_TAG_OPEN + result + filterDate + CMConstants.AND_TAG_CLOSE;

            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string filterStartDate = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;

            result = CMConstants.AND_TAG_OPEN + result + filterStartDate + CMConstants.AND_TAG_CLOSE;

            return result;
        }

        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                        item => item["ReleaseDate"],
                        item => item["PreOrderDate"],
                        item => item["BTKeyList"],
                        item => item["ProductType"]
                        ));
        }

        protected override void Refine(IList<NRCFeaturedTitlesItem> items)
        {
            if (items == null || items.Count == 0)
                return;
        }
    }
}
