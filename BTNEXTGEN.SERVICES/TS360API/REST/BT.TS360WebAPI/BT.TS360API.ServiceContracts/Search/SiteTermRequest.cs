using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts.Search
{
    public class SiteTermBaseRequest : BaseRequest
    {
        public int nPage { get; set; }
        public int nSize { get; set; }
        public bool bSort { get; set; }
        public bool byValue { get; set; }
    }
    public class SiteTermRequest : SiteTermBaseRequest
    {
        public string st { get; set; }
        public SiteTermRequest()
        { }
        public SiteTermRequest(string term, ListSiteTermRequest list)
        {
            this.st = term;
            this.nPage = list.nPage;
            this.nSize = list.nSize;
            this.bSort = list.bSort;
            this.byValue = list.byValue;
        }
    }
    public class ListSiteTermRequest : SiteTermBaseRequest
    {
        public List<string> StList { get; set; }
    }
}
