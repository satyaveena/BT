using System;
using System.Collections.Generic;
using System.Data;

namespace BT.TS360API.ServiceContracts
{
    public class OrderHistoryStatusRequest
    {
        public List<string> AccountNumbers { get; set; }
        public string OrderDate { get; set; }
    }

    public class OrderHistoryShowMonthlyRequest
    {
        public List<string> AccountNumbers { get; set; }
        public string OrderDate { get; set; }
    }

    public class OrderHistoryStatusResponse
    {
        public int TotalUnits { get; set; }
        public Dictionary<string, int> UnitStatusItems { get; set; }
    }

    public class OrderHistoryShowMonthlyResponse
    {
        public string MonthYear1 { get; set; }
        public int TotalUnits1 { get; set; }
        public Dictionary<string, int> UnitStatusItems1 { get; set; }
        public string MonthYear2 { get; set; }
        public int TotalUnits2 { get; set; }
        public Dictionary<string, int> UnitStatusItems2 { get; set; }
    }

    public class RecentOrdersRequest
    {
        public List<string> AccountNumbers { get; set; }

        /// <summary>
        /// Number of orders to return.
        /// </summary>
        public int Size { get; set; }
    }

    public class RecentOrdersResponse
    {
        public List<RecentOrder> RecentOrders { get; set; }
    }

    public class RecentOrder
    {
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string POOrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string OrderStatus { get; set; }
    }

    public class OrgAccountTypesResponse
    {
        public List<BasketAccountType> AccountTypes { get; set; }
        public bool? HasOnlyDashboard { get; set; }    // has more than one dashboard
    }

    public class AccountsToDashboardRequest
    {
        public string DashboardId { get; set; }
        public List<string> AccountIds { get; set; }
    }

    public class DashboardInfo
    {
        public string UserId { get; set; }
        public string DashboardId { get; set; }
        public List<string> AccountIds { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }

    public class SaveDashboardRequest : DashboardInfo
    {
        public string NewDefaultDashboardId { get; set; }
    }

    public class CreateDefaultDashboardResponse : DashboardInfoResponse
    {
        public string ErrorMessage { get; set; }
    }

    public class DashboardInfoResponse
    {
        public string DashboardId { get; set; }
        public bool ResponseStatus { get; set; }
        public string AccountType { get; set; }
        public List<AccountInfoForCustomerDashboard> Accounts { get; set; }        
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }

    public class AccountInfoForCustomerDashboard
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Alias { get; set; }
    }

    public class DashboardSearchRequest
    {
        public string UserId { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }


    public class DashboardResponse
    {
        public List<Dashboard> Dashboards { get; set; }
    }

    public class Dashboard
    {
        public string DashboardId { get; set; }
        public string DashboardName { get; set; }
        public string DashboardType { get; set; }
    }

    //PD
    public class SearchLineResponse
    {
        public bool ShipmentDelivered { get; set; }
        public string DeliveryDate { get; set; }
    }

    public class UPSResponse
    {
        public string ShipTrackingNumber { get; set; }
        public string ShipmentStatus { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string DeliveryDate { get; set; }
    }

    public class DashboardCreateResponse
    {
        public string ErrorMessage { get; set; }
        public DataSet DataSet { get; set; }
    }
}
