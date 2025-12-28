using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    [Serializable]
    public class ComboBoxContext<T> : Dictionary<string, T>
    {
        public int NumberOfItems { get; set; }
        public string Text { get; set; }
    }

    public class UserIdInfo
    {
        public string UserId { get; set; }
    }
}

