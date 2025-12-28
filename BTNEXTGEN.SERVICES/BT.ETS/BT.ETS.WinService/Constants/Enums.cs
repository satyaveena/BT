using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Constants
{
    public enum FileLoggingLevel { INFO, ERROR, STATE };

    public enum QueueItemJobType
    {
        CartReceived,
        DupeCheck,
        Pricing
    }
}
