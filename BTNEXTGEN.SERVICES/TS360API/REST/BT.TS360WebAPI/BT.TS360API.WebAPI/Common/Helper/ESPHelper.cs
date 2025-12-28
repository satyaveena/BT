using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BT.TS360API.WebAPI.Common.Constants;

namespace BT.TS360API.WebAPI.Common.Helper
{
    public class ESPHelper
    {
        public static string GetCategoryType(string apiVersion, string overallScoreType)
        {
            string retVal = RankConstant.CategoryType.LEGACY;

            if (!string.IsNullOrEmpty(apiVersion) && string.Compare(apiVersion.Trim(), RankConstant.APIVersionType.VERSION_3_9, true) >= 0)
            {
                if (!string.IsNullOrEmpty(overallScoreType))
                {
                    retVal = overallScoreType;
                }
            }

            return retVal;
        }
    }
}