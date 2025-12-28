using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class FlagObject
    {
        public bool ShowToc { get; set; }

        public bool ShowMuze { get; set; }

        public FlagObject()
        {
            ShowToc = false;
            ShowMuze = false;
        }
        public FlagObject(bool toc, bool muze)
        {
            ShowToc = toc;
            ShowMuze = muze;
        }
    }
}
