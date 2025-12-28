using System;

namespace BT.Auth.Business.Helper
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
        #endregion
    }
}