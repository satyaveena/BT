using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Profiles
{
    public class SearchForUsersToProxyRequest
    {
        public string UserId { get; set; }
        public string SelectedOrgId { get; set; }
        public string UserSearchKeyword { get; set; }
        public int PageIndex { get; set; }
    }
}
