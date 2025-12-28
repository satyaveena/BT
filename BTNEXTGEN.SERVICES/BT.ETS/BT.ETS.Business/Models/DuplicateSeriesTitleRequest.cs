using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.ETS.Business.Models
{
    public class DuplicateSeriesTitleRequest
    {
        public string OrganizationId { get; set; }
        public List<string> BTKeyList { get; set; }
        public string ProfileId { get; set; }
    }

    public class DuplicateProfiledSeries
    {
        public string ProfiledSeriesId { get; set; }
        public string SeriesId { get; set; }
    }

}