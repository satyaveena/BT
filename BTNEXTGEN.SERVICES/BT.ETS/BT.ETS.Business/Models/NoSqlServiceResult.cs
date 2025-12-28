using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BT.ETS.Business.Constants;

namespace BT.ETS.Business.Models
{
    public class NoSqlServiceResult<T>
    {
        public T Data { get; set; }

        public NoSqlServiceStatus Status { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public NoSqlServiceResult()
        {
            Status = NoSqlServiceStatus.Success;
        }
    }
}