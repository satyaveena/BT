using BT.TS360API.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Helpers
{
    internal static class GridDataHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectiveDate"></param>
        /// <param name="expirationDate"></param>
        /// <returns></returns>
        public static bool ValidEffectiveExpirationDate(DateTime? effectiveDate, DateTime? expirationDate)
        {
            if (effectiveDate == null && expirationDate == null)
                return true;

            if (effectiveDate != null && effectiveDate >= DateTime.Now)
                return false;

            if (expirationDate != null && expirationDate < DateTime.Now)
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectiveDate"></param>
        /// <param name="expirationDate"></param>
        /// <returns></returns>
        public static bool IsFutureDate(DateTime? effectiveDate)
        {
            if (effectiveDate != null && effectiveDate >= DateTime.Now)
                return true;

            return false;
        }

        public static CartGridLineFieldCode LookUpGridFieldCode(List<CommonBaseGridUserControl.UIGridCode> gridCodes,
            string gridCodeId, bool isShowCode = true)
        {
            var gridCode = gridCodes.FirstOrDefault(x => x.ID == gridCodeId);
            if (gridCode != null)
            {
                var fieldCode = new CartGridLineFieldCode();
                fieldCode.GridCodeId = gridCodeId;
                fieldCode.GridFieldId = gridCode.GridFieldID;
                fieldCode.GridCodeValue = isShowCode ? gridCode.Code : gridCode.Literal;
                fieldCode.IsFreeText = false;
                fieldCode.IsExpired = !ValidEffectiveExpirationDate(gridCode.EffectiveDate, gridCode.ExpirationDate);
                fieldCode.IsDisabled = gridCode.Disable;
                fieldCode.IsExpiredOrFutureDate = fieldCode.IsExpired || IsFutureDate(gridCode.EffectiveDate);
                fieldCode.IsFutureDate = IsFutureDate(gridCode.EffectiveDate);
                return fieldCode;
            }
            return null;
        }

        public static CartGridLineFieldCode LookUpCallNumberGridFieldCodeInOrg(List<CommonBaseGridUserControl.UIGridField> uiGridFields)
        {
            var fieldCode = new CartGridLineFieldCode();
            var callNumberGridField = uiGridFields.FirstOrDefault(x => x.IsFreeText);
            if (callNumberGridField != null)
            {
                fieldCode.GridFieldId = callNumberGridField.ID;
                fieldCode.IsFreeText = true;
            }
            return fieldCode;
        }
    }
}
