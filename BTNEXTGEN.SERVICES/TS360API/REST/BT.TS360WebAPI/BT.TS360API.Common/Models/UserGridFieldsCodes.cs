using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class UserGridFieldsCodes
    {
        public List<CommonBaseGridUserControl.UIUserGridField> UserGridFields { get; set; }

        public List<CommonBaseGridUserControl.UIGridCode> UserGridCodes { get; set; }

        public int DefaultQuantity { get; set; }

        public UserGridFieldsCodes()
        {
            DefaultQuantity = 0;
        }
    }
}
