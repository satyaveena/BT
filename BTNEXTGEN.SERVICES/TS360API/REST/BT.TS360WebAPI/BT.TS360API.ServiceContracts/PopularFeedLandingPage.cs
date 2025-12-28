using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class PopularFeedLandingPage
    {
        public string TargetingText { get; set; }

        public string HeaderTextEList { get; set; }
        public string MoreELists { get; set; }

        public List<TS_ShortEListItem> EList { get; set; }

        
        public string HeaderTextNewsFeeds { get; set; }
        public string MoreNewsFeeds { get; set; }

        public List<string> NewsFeedsList { get; set; }
    }

    public class TS_ShortEListItem
    {
        public string Title { get; set; }
        public string PostBackUrl { get; set; } 
    }
}
