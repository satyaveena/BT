using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Models
{
    public class DupCheckDetailResult
    {
        public string BTKey { get; set; }
        public string DupCheckStatusType { get; set; }
        public List<DupCheckCartInfo> DupCheckCartInfo { get; set; }

        public List<DupCheckSeriesInfo> DupCheckSeriesInfo { get; set; }
    }

    public class DupCheckCartInfo
    {
        public string CartId { get; set; }
        public string CartName { get; set; }
        public string CartStatus { get; set; }
        public DateTime CartLastUpdatedDateTime { get; set; }
        public int Quantity { get; set; }
        public string BasketOwnerId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string AccountNumber { get; set; }
        public List<GridLineInfor> GridLines { get; set; }
        public List<OrderInfor> Orders { get; set; }

    }
    public class GridLineInfor
    {
        public int Quantity { get; set; }
        public string CollectionCode { get; set; }
        public string AgencyCode { get; set; }
        public string ItemTypeCode { get; set; }
        public string CallNumberText { get; set; }
        public string GridFieldName1 { get; set; }
        public string GridCode1 { get; set; }
        public string GridFieldName2 { get; set; }
        public string GridCode2 { get; set; }
        public string GridFieldName3 { get; set; }
        public string GridCode3 { get; set; }
        public string GridFieldName4 { get; set; }
        public string GridCode4 { get; set; }
        public string GridFieldName5 { get; set; }
        public string GridCode5 { get; set; }
        public string GridFieldName6 { get; set; }
        public string GridCode6 { get; set; }        
    }
    public class OrderInfor
    {
        public string OrderNumber { get; set; }
        public string Warehouse { get; set; }
        public int Shipped { get; set; }
        public int InProcess { get; set; }
        public int Cancelled { get; set; }
        public int Backordered { get; set; }
        public int Reserved { get; set; }
    }
    public class DupCheckSeriesInfo
    {
        public string ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string SeriesId { get; set; }
        public string SeriesName { get; set; }
        public string AccountNumber { get; set; }
        public string ProgramType { get; set; }
        public string ProfileType { get; set; }
        public int Quantity { get; set; }
        public char IsEnabled { get; set; }
        public List<OrderSeriesInfor> Orders { get; set; }

    }
    public class OrderSeriesInfor
    {
        public string FormatPreferencesPrimary { get; set; }
        public int FormatPreferencesPrimaryQuantity { get; set; }
        public string FormatPreferencesSecondary { get; set; }
        public int FormatPreferencesSecondaryQuantity { get; set; }
        public string PO { get; set; }
        public string StartDate { get; set; }
        public string ShippingPreference { get; set; }
    }

    public class SeriesProducts
    { 
        public string SeriesID { get; set; }
      
        public string StartData { get; set; }
      
        public string AutoShipIndicator { get; set; }

        public class SeriesProductsRecords
        {
            public SeriesProductsRecords()
            {
                SeriesProductsData = new List<SeriesProducts>();
            }
            public List<SeriesProducts> SeriesProductsData { get; set; }
        }
    }
}
