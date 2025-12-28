using BT.TS360API.ServiceContracts.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class SearchCartLinesCriteria : SearchCartCriteria
    {
        public SearchCartLinesCriteria(string cartId, string userId)
        {
            this.CartId = cartId;
            this.UserId = userId;
        }

        public SearchCartLinesCriteria(string cartId, string userId, SearchCartCriteria criteria)
            : this(cartId, userId)
        {
            if (criteria != null)
            {
                this.IsQuickCartDetails = criteria.IsQuickCartDetails;
                this.FacetPath = criteria.FacetPath;
                this.Keyword = criteria.Keyword;
                this.KeywordType = criteria.KeywordType;
                this.PageNumber = criteria.PageNumber;
                this.PageSize = criteria.PageSize;
                this.SortBy = criteria.SortBy;
                this.QuickSortBy = criteria.QuickSortBy;
                this.SortDirection = criteria.SortDirection;
                this.MatchingBtkeys = criteria.MatchingBtkeys;
            }
        }
        public string CartId { get; set; }
        public string UserId { get; set; }
    }
}
