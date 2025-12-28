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
    public class NewReleaseCalendar : CMListBase<NewReleaseCalendarItem>
    {
        string RequestDomainName { get; set; }
        string Month { get; set; }
        string Year { get; set; }
        List<string> ProductTypesList { get; set; }
        public NewReleaseCalendar(string month, string year, List<string> productTypesList, string requestDomainName)
        {
            Month = month;
            Year = year;
            ProductTypesList = productTypesList;
            RequestDomainName = requestDomainName;
            HasAdName = false;
            HasIsDefault = false;   //TODO: add IsDefault filter
            RowLimit = 10000;
        }
        protected override string GetListName()
        {
            return CMListNameConstants.NewReleaseCalendar;
        }

        protected override string AddWhereCamlTo(string condition)
        {
            string result = condition;
            //Get product type

            result = result + ContentManagementHelper.BuildCamlString(ProductTypesList, CMFieldNameConstants.ProductType);

            //get product type is null
            string productTypeNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.ProductType, CMConstants.IsNull_TAG_CLOSE);
            result = CMConstants.OR_TAG_OPEN + result + productTypeNull + CMConstants.OR_TAG_CLOSE;


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

        protected override void Refine(IList<NewReleaseCalendarItem> items)
        {
            if (items == null || items.Count == 0)
                return;
            else
            {
                var listProductInfo = new List<ProductInfo>();
                var refineItems = new List<NewReleaseCalendarItem>();

                foreach (var item in items)
                {
                    if (string.IsNullOrEmpty(item.ProductType) || item.PreOrderDate == DateTime.MinValue)
                    {
                        // call NoSql service to get ProductType & PreOrderDate
                        listProductInfo = CommonHelper.GetNRCProductInfo(item.BTKeys);

                        foreach (var productInfo in listProductInfo)
                        {
                            if (ProductTypesList.Contains(productInfo.ProductType))
                            {
                                var itemNRC = new NewReleaseCalendarItem
                                {
                                    StreetDate = item.StreetDate,
                                    PreOrderDate = productInfo.PreOrderDate,
                                    ProductType = productInfo.ProductType,
                                    BTKeyList = productInfo.BTKey
                                };
                                refineItems.Add(itemNRC);
                            }
                        }
                    }
                    else
                        refineItems.Add(item);
                }

                items.Clear();
                foreach (var item in refineItems)
                {
                    items.Add(item);
                }
            }
        }
    }
}