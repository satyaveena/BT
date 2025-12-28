using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class _RankConstant
    {
        public class JobType
        {
            public string RANK = "rank";
        }

        public class JobStatus
        {
            public string RETURNED = "returned";
        }

        public class RankType
        {
            public string AUTHORSERIES = "authorSeries";
            public string AUTHORBISAC = "authorBisac";
        }
    }
}