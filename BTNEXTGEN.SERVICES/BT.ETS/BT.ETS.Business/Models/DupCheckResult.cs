using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Models
{
    public class DupCheckResult
    {
        public List<ProductDupCheckStatus> Items { get; set; }
        public List<ErrorItem> ErrorItems { get; set; }

        public DupCheckResult()
        {
            Items = new List<ProductDupCheckStatus>();
            ErrorItems = new List<ErrorItem>();
        }
    }
    public class ProductDupCheckStatus
    {
        public string BTKey { get; set; }
        public List<string> DupCheckStatus { get; set; }
    }

    public class DupCheckDataResult
    {
        public DataSet Data { get; set; }
        public string OrgId { get; set; }
        public string SeriesDupeCheckType { get; set; }
        public string OrdersDupeCheckType { get; set; }
        public DupCheckResult DupCheckResult { get; set; }
        public string DupCheckPreferenceDownloadCart { get; set; }
    }

    public class OrdersDupCheckRequest
    {
        public string OrgId { get; set; }
        public string UserId { get; set; }
        public string BasketId { get; set; }

        public List<string> BTKeys { get; set; }

        public string CartCheckType { get; set; }   // MyCarts or AllCarts
        public string OrderCheckType { get; set; }  // MyAccounts or AllAccounts
        public string DownloadedCheckType { get; set; } // IncludeWCarts or IncludeWOrders

        public List<string> UserAccounts { get; set; }
    }

    public class DuplicateItem
    {
        public string BTKey { get; set; }
        public bool IsDuplicated { get; set; }
    }

    public class OrdersDupCheckResponse
    {
        public List<DuplicateItem> DuplicateItems { get; set; }
    }

}
