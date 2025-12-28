using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Search
{
    public class BTListsArgRequest : BaseRequest
    {
        public BTListsArg arg { get; set; }
    }

    public class BTListsArg
    {
        public List<int> EListCatIds { get; set; }                         
    }
     
    public class BTListReturn
    {
        public List<EListCategory> EListCats { get; set; }
    }
     
    public class EListCategory
    {
        public int EListCatId { get; set; }
        public int PubCount { get; set; }
        public EListCategory()
        { }
        public EListCategory(int id, int count)
        {
            EListCatId = id;
            PubCount = count;
        }
    }
}
