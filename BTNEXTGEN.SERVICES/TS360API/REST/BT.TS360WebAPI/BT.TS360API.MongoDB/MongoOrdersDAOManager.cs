using BT.TS360API.MongoDB.DataAccess;
using BT.TS360API.ServiceContracts;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;
using BT.TS360API.MongoDB.Common;

namespace BT.TS360API.MongoDB
{
    public class MongoOrdersDAOManager
    {
        public static Dictionary<string, bool> GetOrderDuplicates(OrdersDupCheckRequest request)
        {
            var results = new Dictionary<string, bool>();

            if (request != null && request.BTKeys != null && request.BTKeys.Count > 0)
            {
                // init results
                foreach (var btKey in request.BTKeys)
                {
                    if (btKey != null && !results.ContainsKey(btKey))
                        results.Add(btKey, false);
                }
                //if ((string.Equals(request.OrderCheckType, DefaultDuplicateOrders.MyAccounts.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(request.OrderCheckType, DefaultDuplicateOrders.AllAccounts.ToString(), StringComparison.OrdinalIgnoreCase)) || string.Equals(request.DownloadedCheckType, "IncludeWOrders", StringComparison.OrdinalIgnoreCase))
                //{
                    // get Dup O from MongoDB
                    var bsonDocuments = OrdersDAO.Instance.GetOrderDuplicates(request);
                    if (bsonDocuments != null)
                    {
                        foreach (var document in bsonDocuments)
                        {
                            var docBTKey = document.Contains(FieldNames.BTKey) ? document[FieldNames.BTKey].AsString : null;

                            if (!string.IsNullOrWhiteSpace(docBTKey) && results.ContainsKey(docBTKey))
                                results[docBTKey] = true;
                        }
                    }
                //}
            }

            return results;
        }
    }
}
