using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Common.Constants
{
    public class RankConstant
    {
        public class APIVersionType
        {
            public const string VERSION_3_4 = "3.4";
            public const string VERSION_3_9 = "3.9";
        }

        public class CategoryType
        {
            public const string LEGACY = "Overall";
        }
        public class JobType
        {
            public const string RANK = "rank";
        }

        public class JobStatus
        {
            public const string RETURNED = "returned";
        }

        public class RankType
        {
            public const string AUTHORSERIES = "authorSeries";
            public const string AUTHORBISAC = "authorBisac";
        }

        public class ESPType
        {
            public const string RANK = "RANK";
            public const string DIST = "DIST";
        }

        public class ESPSubmissionValidationMessage
        {
            public const string REQUIRED_CART_ID = "REQUIRED_CART_ID";
            public const string REQUIRED_USER_ID = "REQUIRED_USER_ID";
            public const string INVALID_ESP_TYPE = "INVALID_ESP_TYPE";
        }
    }
}