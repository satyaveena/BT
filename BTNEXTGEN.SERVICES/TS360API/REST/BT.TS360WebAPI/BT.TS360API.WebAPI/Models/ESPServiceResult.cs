using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BT.TS360API.WebAPI.Common.Constants;

namespace BT.TS360API.WebAPI.Models
{
    public class ESPServiceResult<T>
    {
         public T Data { get; set; }

         public ESPServiceStatus Status { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public ESPServiceResult()
        {
            Status = ESPServiceStatus.Success;
            ErrorCode = "";
            ErrorMessage = "";
        }
    }
}