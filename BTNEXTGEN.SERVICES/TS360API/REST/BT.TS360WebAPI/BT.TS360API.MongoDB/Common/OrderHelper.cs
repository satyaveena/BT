using BT.TS360API.ServiceContracts.Search;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.MongoDB.Common
{
    public static class OrderHelper
    {
        public static SortDefinition<BsonDocument> BuildCommonSort(string sortBy, string sortDirection)
        {
            if (sortBy != null && sortDirection != null)
            {
                if(string.Equals("OrderDate", sortBy, StringComparison.OrdinalIgnoreCase))
                {
                    return sortDirection.ToLower() == "desc" ? Builders<BsonDocument>.Sort.Descending(sortBy).Descending("ISBN")
                        : Builders<BsonDocument>.Sort.Ascending(sortBy).Ascending("ISBN");
                }
                else
                {
                    return sortDirection.ToLower() == "desc" ? Builders<BsonDocument>.Sort.Descending(sortBy)
                        : Builders<BsonDocument>.Sort.Ascending(sortBy);
                }
                
            }
            return null;
        }

        public static BsonDocument BuildSkip(int  pageSize, int pageNumber )
        {
           return new BsonDocument("$skip", pageSize*(pageNumber <= 0 ? 0 : pageNumber - 1));
        }

         public static BsonDocument BuildLimit(int  pageSize)
        {
            return new BsonDocument("$limit", pageSize);
        }


        public static SortDefinition<BsonDocument> BuildSortDefinition(SortExpression sortExpression)
        {
            if (sortExpression != null)
            {
                if (sortExpression.SortDirection == ServiceContracts.Search.SortDirection.Descending)
                    return Builders<BsonDocument>.Sort.Descending(sortExpression.SortField);

                return Builders<BsonDocument>.Sort.Ascending(sortExpression.SortField);
            }

            return null;
        }

        public static bool CheckViewByIsbnUPC(List<string> list)
        {
            var result = false;

            if (list != null && list.Count > 0)
            {
                if (list[0].Contains("viewByIsbn/Upc:"))
                    result = true;
            }

            return result;
        }
    }
}
