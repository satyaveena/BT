using BT.TS360API.Logging;
using BT.TS360API.MongoDB;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Order;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360API.ExternalServices;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Services.Services
{
    public partial class CustomerService
    {
        public async Task<OrderSearchLinesResponseResult> GetSearchLines(OrderSearchLinesRequest  request)
        {
            return await OrderLinesDAOManager.GetSearchLines(request);
        }
        
        public async Task<SearchOrdersResult> GetSearchOrders(OrderSearchLinesRequest request)
        {
            return await OrderLinesDAOManager.GetSearchOrders(request);
        }

        public async Task<long> GetLinesOrOrdersSearchCount(OrderSearchLinesRequest request)
        {
            long resultCount;
            if (request.IsOrderLineSearch == true)
            {
                // count total items of Order Lines search results
                resultCount = await OrderLinesDAOManager.GetSearchLinesResultCount(request);
            }
            else
            {
                // count total items of Order search results
                resultCount = await OrderLinesDAOManager.GetSearchOrdersResultCount(request);
            }

            return resultCount;
        }

        internal async Task<OrderSearchSummaryResponse> GetOrderSearchSummary(OrderLineRequest request)
        {
            return await OrderLinesDAOManager.GetOrderSearchSummary(request);
        }

        internal async Task<LineStatusResponse> GetLineStatus(string orderLineId)
        {
            return await OrderLinesDAOManager.GetLineStatus(orderLineId);
        }

        internal async Task<SearchLineFacetsResponse> SearchLineFacets(OrderLineRequest request)
        {
            return await OrderLinesDAOManager.SearchLineFacets(request);
        }

        internal async Task<SearchOrderFacetsResponse> SearchOrderFacets(OrderLineRequest request)
        {
            return await OrderLinesDAOManager.SearchOrderFacets(request);
        }

        internal async Task<OrderSearchExportResponse> SearchLineExport(OrderSearchLinesRequest request)
        {
            return await OrderLinesDAOManager.SearchLineExport(request);
        }

        internal async Task<OrderSearchExportResponse> SearchOrderExport(OrderSearchLinesRequest request)
        {
            return await OrderLinesDAOManager.SearchOrderExport(request);
        }

        public async Task<UPSResponse> GetShippingStatus(string ShipTrackingNumber)
        {
            var result = await CommonUPSHelper.Instance.GetUPSData(ShipTrackingNumber);

            if (string.Equals(result.ShipmentStatus, "Delivered", StringComparison.OrdinalIgnoreCase))
            {
                await OrderLinesDAOManager.UpdateDeliveryDetails(ShipTrackingNumber, true, result.DeliveryDate);
            }
            
            return result;
        }
    }
}