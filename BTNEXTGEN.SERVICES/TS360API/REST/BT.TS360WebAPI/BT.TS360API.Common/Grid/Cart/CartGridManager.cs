using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BT.TS360API.Common.Grid.Cart
{
    public class CartGridManager
    {
        private List<string> _lineItemIds = new List<string>();
        private Dictionary<string, List<CommonCartGridLine>> _dicCartGridLineList = new Dictionary<string, List<CommonCartGridLine>>();
        private List<string> _deletedGridLineIDs = new List<string>();

        private static readonly object SyncRoot = new Object();
        private CartGridManager()
        {

        }

        public static CartGridManager Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    CartGridManager _instance = HttpContext.Current==null? new CartGridManager(): HttpContext.Current.Items["CartGridManager"] as CartGridManager;
                    if (_instance == null)
                    {
                        _instance = new CartGridManager();
                        HttpContext.Current.Items.Add("CartGridManager", _instance);
                    }

                    return _instance;
                }
            }
        }

        public Dictionary<string, List<CommonCartGridLine>> LoadCartLineItemGridLinesIII(string userId, string orgId, List<string> lineItemIds)
        {
            var fieldCodeDs = DistributedCacheHelper.GetGridFieldsCodesForOrgFromDao(orgId);
            var activeGridFields = DistributedCacheHelper.RefineDsAndGetActiveUiGridField(fieldCodeDs, true);
            //var activeGridFields = DistributedCacheHelper.GetActiveGridFieldsForOrg(orgId, true);
            
            var userGridFieldsCodes = UserGridFieldsCodesManager.Instance.GetUserGridFieldsCodesIII(userId, orgId);
            var dict = GridDAOManager.Instance.GetCartGridLinesNewGrid(lineItemIds);
            foreach (var lineItemId in lineItemIds)
            {
                if (dict.ContainsKey(lineItemId))
                {
                    var cartGridLines = dict[lineItemId];
                    cartGridLines = cartGridLines.OrderBy(x => x.Sequence).ToList();
                    foreach (var gridLine in cartGridLines)
                    {
                        RefineCartGridLineFieldCodesIII(gridLine, activeGridFields, userGridFieldsCodes);
                    }
                    dict[lineItemId] = cartGridLines;
                }
                else
                {
                    dict.Add(lineItemId, new List<CommonCartGridLine>());
                }
            }
            return dict;
        }
        private void RefineCartGridLineFieldCodesIII(CommonCartGridLine gridLine, List<CommonBaseGridUserControl.UIGridField> activeGridFields, UserGridFieldsCodes userGridFieldsCodes)
        {
            var refinedGridFieldCodes = new List<CartGridLineFieldCode>();

            foreach (var gf in activeGridFields)
            {
                var newFc = new CartGridLineFieldCode();
                newFc.IsFreeText = gf.IsFreeText;
                newFc.GridFieldId = gf.ID;

                var isShowCode = true;
             
                foreach (var gfc in gridLine.GridFieldCodeList)
                {
                    if (!gfc.IsFreeText)
                    {
                        if (userGridFieldsCodes != null)
                        {
                            var fc = GridDataHelper.LookUpGridFieldCode(userGridFieldsCodes.UserGridCodes, gfc.GridCodeId, isShowCode);
                            if (fc != null && fc.GridFieldId == gf.ID)
                            {
                                fc.IsAuthorized = true;
                                newFc = fc;
                                break;
                            }
                        }

                        var fullFc = GridDataHelper.LookUpGridFieldCode(gf.UIGridCodes, gfc.GridCodeId, isShowCode);
                        if (fullFc != null && fullFc.GridFieldId == gf.ID)
                        {
                            newFc = fullFc;
                            newFc.IsAuthorized = false;
                            break;
                        }
                        //if (!string.IsNullOrEmpty(gfc.GridCodeId))
                        //{
                        //    hasUnAuthorizedGridCode = true;
                        //}
                    }
                    else
                    {
                        var freeTextFc = GridDataHelper.LookUpCallNumberGridFieldCodeInOrg(activeGridFields);
                        if (!string.IsNullOrEmpty(freeTextFc.GridFieldId) && freeTextFc.GridFieldId == gf.ID)
                        {
                            newFc = freeTextFc;
                            freeTextFc.GridTextValue = gfc.GridTextValue;
                            freeTextFc.IsAuthorized = true;
                            break;
                        }
                    }
                }

                newFc.GridFieldType = CommonHelper.ConvertToGridFieldType(gf.GridFieldType);
                refinedGridFieldCodes.Add(newFc);
            }
            gridLine.GridFieldCodeList = refinedGridFieldCodes;
            gridLine.IsAuthorized = !refinedGridFieldCodes.Exists(x => x.IsAuthorized == false && !string.IsNullOrEmpty(x.GridCodeId));
        }
    }
}
