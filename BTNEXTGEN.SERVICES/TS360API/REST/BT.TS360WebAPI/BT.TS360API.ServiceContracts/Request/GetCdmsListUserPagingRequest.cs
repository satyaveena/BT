
using System.Collections.Generic;


namespace BT.TS360API.ServiceContracts.Request
{
    public class GetCdmsListUserPagingRequest : BaseRequest
    {
        public string cdmsListId { get; set; }     
        public int pageSize { get; set; }     
        public int pageIndex { get; set; }     
        public string sortBy { get; set; }     
        public string sortDirection { get; set; }     
    }
}
