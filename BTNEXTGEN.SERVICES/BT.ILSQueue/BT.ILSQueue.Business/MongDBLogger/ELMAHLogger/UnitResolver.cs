using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.MongDBLogger.ELMAHLogger
{
    public class UnitResolver
    {
        #region Public Method
        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="valueWithUnit"></param>
        /// <returns>Int64</returns>
        public Int64 Resolve(String valueWithUnit)
        {
            if (valueWithUnit == null)
            {
                return 0;
            }

            Int32 result;

            if (Int32.TryParse(valueWithUnit, out result)) return result;

            Regex regex = new Regex(@"^(\d+)(k|MB){0,1}$");
            Match match = regex.Match(valueWithUnit);

            if (!match.Success) return result;
            Int32 value = Int32.Parse(match.Groups[1].Value);
            Int32 multiplier = GetMultiplier(match.Groups[2].Value);
            result = value * multiplier;

            return result;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// GetMultiplier
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>Int32</returns>
        private Int32 GetMultiplier(String unit)
        {
            switch (unit)
            {
                case "k":
                    return 1000;
                case "MB":
                    return 1024 * 1024;
                default:
                    return 0;
            }
        }
        #endregion
    }
}
