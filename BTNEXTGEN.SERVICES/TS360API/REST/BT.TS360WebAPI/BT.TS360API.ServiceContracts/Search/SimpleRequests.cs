using System.Collections.Generic;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.Search
{
    public class BaseSimple: BaseRequest
    {
        public string UserId { get; set; }
    }
    
    public class SimpleOne:BaseSimple
    {
        public string Param1 { get; set; }
    }
    public class SimpleOneInt : BaseSimple
    {
        public int Param1 { get; set; }
    }
    public class LoadFacetRequest : SimpleOne
    {
        public string OrgId { get; set; }

        public MarketType? MarketType { get; set; }
        public string[] ProductType { get; set; }
        public bool IsWfeFarmCacheAvailable { get; set; }
        public SearchByIdData SearchData { get; set; }
        public SearchArguments SearchArguments { get; set; }
    }
    public class SimpleTwo : SimpleOne
    {
        public string Param2 { get; set; }
    }
    public class SimpleThree : SimpleTwo
    {
        public string Param3 { get; set; }
    }

    public class SimpleTwoArrStr : SimpleOne
    {
        public string[] Param2 { get; set; }
    }
    public class TwoParam : BaseRequest
    {
        public string Param1 { get; set; }
        public string Param2 { get; set; }
    }
    public class ListSimpleTwo : BaseSimple
    {
        public List<TwoParam> List { get; set; }
    }

}
