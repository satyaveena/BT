using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.ExternalServices
{
    public class ServiceHelper
    {
    }

    public static class ExtSvcDataAccessHelper
    {
        public static string ConvertToString(object obj)
        {
            if (null != obj && DBNull.Value != obj)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        public static char ConvertToChar(object obj)
        {
            char returnValue = ' ';
            if (null != obj && DBNull.Value != obj)
            {
                char.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ConvertToInt(object obj)
        {
            int returnValue = 0;
            if (null != obj)
            {
                int.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean(object obj)
        {
            bool returnValue = false;
            if (null != obj && obj != DBNull.Value)
            {
                bool.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        public static decimal ConvertToDecimal(object obj)
        {
            decimal returnValue = 0;
            if (null != obj)
            {
                decimal.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        public static float ConvertToFloat(object obj)
        {
            float returnValue = 0;
            if (null != obj)
            {
                float.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }
    }
}
