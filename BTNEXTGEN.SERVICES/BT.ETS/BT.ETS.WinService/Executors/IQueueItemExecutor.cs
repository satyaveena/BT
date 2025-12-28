using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Executors
{
    public interface IQueueItemExecutor
    {
        Task ExecuteRequest(ETSQueueItem etsQueueItem);
    }
}
