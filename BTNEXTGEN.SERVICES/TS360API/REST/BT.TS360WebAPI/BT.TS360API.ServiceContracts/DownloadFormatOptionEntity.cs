using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class DownloadFormatOptionEntity
    {
        public string Format { get; set; }

        public string Selection1 { get; set; }

        public bool Check1 { get; set; }
                
        public bool SaveAsDownloaded { get; set; }

        
    }
}
