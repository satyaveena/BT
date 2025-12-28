using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Constants
{
    public enum NoSqlServiceStatus
    {
        Success = 0,
        Fail = 1
    }

    public enum QueueProcessState
    {
        New = 0,
        InProcess = 1,
        Success = 2,
        Failed = 3,
        Loading = 4
    }
}
