using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class GetWhatHotAndPopularFeedDataResponse
    {
        public WhatHotLandingPage WhatHot { get; set; }

        public PopularFeedLandingPage PopularFeed { get; set; }
    }
}
