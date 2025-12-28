using System;

namespace BT.CDMS.Business.Helper
{
    /// <summary>
    /// DataAccessHelper
    /// </summary>
    public class DataAccessHelper
    {
        #region Method
        /// <summary>
        /// ConvertToInt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>int</returns>
        public static int ConvertToInt(object obj)
        {
            int returnValue = 0;
            if (null != obj)
            {
                Int32.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToString(object obj)
        {
            if (null != obj && DBNull.Value != obj)
            {
                return obj.ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Convert an object to bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ConvertToBool(object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;

            var resultString = obj.ToString();
            switch (resultString)
            {
                case "1":
                    resultString = "True";
                    break;
                case "0":
                    resultString = "False";
                    break;
            }

            bool result;
            return bool.TryParse(resultString, out result) && result;
        }
        #endregion
    }
}