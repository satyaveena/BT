using System.Collections.Generic;

namespace BT.TS360API.Common.Search
{
    public abstract class SearchResults<T>
    {
        public IList<T> Items { get; set; }
        public int TotalRowCount { get; set; }
    }
}
