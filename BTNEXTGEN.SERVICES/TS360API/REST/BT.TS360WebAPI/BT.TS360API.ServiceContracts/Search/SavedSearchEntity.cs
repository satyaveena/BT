using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Search
{
    public class SavedSearchEntity
    {
        public string UserId { get; set; }
        public string SavedSearchId { get; set; }
        public string SavedSearchName { get; set; }
        public string SearchCriteria { get; set; }
        public int SearchFrom { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
