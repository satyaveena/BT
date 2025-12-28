using BT.TS360API.Common.Business;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Exceptions;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Models;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CartMixMode = BT.TS360Constants.CartMixMode;

namespace BT.TS360API.Common.Grid.Cart
{
    public class CartGridDataAccessManager
    {
        private static readonly object SyncRoot = new Object();


        private CartGridDataAccessManager()
        {

        }

        public static CartGridDataAccessManager Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    CartGridDataAccessManager _instance = HttpContext.Current == null ? new CartGridDataAccessManager() : HttpContext.Current.Items["CartGridDataAccessManager"] as CartGridDataAccessManager;
                    if (_instance == null)
                    {
                        _instance = new CartGridDataAccessManager();
                        HttpContext.Current.Items.Add("CartGridDataAccessManager", _instance);
                    }

                    return _instance;
                }
            }
        }

        public Dictionary<string, List<CommonCartGridLine>> GetCartGridLinesFromDataSetNewGrid(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows == null)
            {
                return null;
            }

            var dict = new Dictionary<string, List<CommonCartGridLine>>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var line = new CommonCartGridLine();
                string lineItemId = DataAccessHelper.ConvertToString(row["BasketLineItemID"]);
                line.LineItemId = lineItemId;
                line.CartGridLineId = DataAccessHelper.ConvertToString(row["BasketLineItemGridID"]);
                line.Quantity = DataAccessHelper.ConvertToInt(row["Quantity"]);
                line.Sequence = DataAccessHelper.ConvertToInt(row["Sequence"]);
                line.LastModifiedByUserName = DataAccessHelper.ConvertToString(row["UpdatedBy"]);
                line.GridFieldCodeList = new List<CartGridLineFieldCode>();
                line.IsAuthorized = true;

                line.GridFieldCodeList.Add(new CartGridLineFieldCode
                {
                    CartGridLineId = line.CartGridLineId,
                    GridCodeId = DataAccessHelper.ConvertToString(row["AgencyCodeID"]),
                    IsFreeText = false,
                    GridFieldType = GridFieldType.AgencyCode
                });

                line.GridFieldCodeList.Add(new CartGridLineFieldCode
                {
                    CartGridLineId = line.CartGridLineId,
                    GridCodeId = DataAccessHelper.ConvertToString(row["ItemTypeID"]),
                    IsFreeText = false,
                    GridFieldType = GridFieldType.ItemType
                });

                line.GridFieldCodeList.Add(new CartGridLineFieldCode
                {
                    CartGridLineId = line.CartGridLineId,
                    GridCodeId = DataAccessHelper.ConvertToString(row["CollectionID"]),
                    IsFreeText = false,
                    GridFieldType = GridFieldType.Collection
                });

                for (int i = 1; i <= 6; i++)
                {
                    string gridCode = "UserCode" + i;

                    var fieldCode = new CartGridLineFieldCode
                    {
                        CartGridLineId = line.CartGridLineId,
                        GridCodeId = DataAccessHelper.ConvertToString(row[gridCode + "ID"]),
                        IsFreeText = false,
                        GridFieldType = CommonHelper.ConvertToGridFieldType(gridCode)
                    };

                    line.GridFieldCodeList.Add(fieldCode);
                }
                line.CallNumberText = DataAccessHelper.ConvertToString(row["CallNumberText"]);
                line.GridFieldCodeList.Add(new CartGridLineFieldCode
                {
                    CartGridLineId = line.CartGridLineId,
                    GridTextValue = line.CallNumberText,
                    IsFreeText = true,
                    GridFieldType = GridFieldType.CallNumber
                });

                if (!dict.ContainsKey(lineItemId))
                    dict.Add(lineItemId, new List<CommonCartGridLine>());

                dict[lineItemId].Add(line);
            }
            return dict;
        }
        
    }
}
