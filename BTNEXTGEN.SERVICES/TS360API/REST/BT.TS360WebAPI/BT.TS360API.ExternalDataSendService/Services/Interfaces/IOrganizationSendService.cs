using BT.TS360API.ExternalDataSendService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.TS360API.ExternalDataSendService.Interfaces
{
    public interface IOrganizationSendService
    {
        Task<bool> SendOrganizationAsync(string orgId);
    }
}
