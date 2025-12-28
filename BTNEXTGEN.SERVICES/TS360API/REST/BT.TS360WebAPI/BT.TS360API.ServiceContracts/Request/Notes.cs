
using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class NotesRequest : BaseRequest
    {
        public string cartId { get; set; }
        public List<string> btkeys { get; set; }
    }
}
