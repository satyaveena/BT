using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.SqlServer.Server;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Exceptions;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Models;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using BT.TS360API.Logging;
using BT.TS360API.Cache;
using System.Web;

namespace BT.TS360API.Common.Grid
{
    public class GridDataAccessManager
    {
        private GridDataAccessManager()
        {

        }
        private static volatile GridDAOManager _gridDaoManager;
        private static readonly object SyncRoot = new Object();
        //private static readonly VelocityCacheLevel CacheLevel = GridCacheConstants.CommonGridCacheLevel;      


        public static GridDataAccessManager Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    GridDataAccessManager _instance = HttpContext.Current == null ? new GridDataAccessManager() : HttpContext.Current.Items["GridDataAccessManager"] as GridDataAccessManager;
                    if (_instance == null)
                    {
                        _instance = new GridDataAccessManager();
                        _gridDaoManager = GridDAOManager.Instance;
                        HttpContext.Current.Items.Add("GridDataAccessManager", _instance);
                    }

                    return _instance;
                }
            }
        }

      public UserGridFieldsCodes GetUserGridFieldsCodes(string userId, string orgId)
        {
            try
            {
                var defaultQuantity = 0;
                var ds = GridDAOManager.Instance.GetUserGridFieldsCodes(userId, orgId, out defaultQuantity);
                var userGridFieldCodes = RefineUserGridFieldsCodes(ds);
                userGridFieldCodes.DefaultQuantity = defaultQuantity;

                return userGridFieldCodes;
            }
            catch (Exception ex)
            {
                throw new CartGridSaveFailedException(ex.Message);
            }
        }
        private UserGridFieldsCodes RefineUserGridFieldsCodes(DataSet dsFieldCode)
        {
            var userGridFieldCodes = new UserGridFieldsCodes();
            if (dsFieldCode != null && dsFieldCode.Tables.Count == 2)
            {
                var fieldTable = dsFieldCode.Tables[0];
                var codeTable = dsFieldCode.Tables[1];
                var gridFields = new List<CommonBaseGridUserControl.UIUserGridField>();
                foreach (DataRow fieldRow in fieldTable.Rows)
                {
                    var field = new CommonBaseGridUserControl.UIUserGridField();
                    field.GridFieldID = fieldRow["GridFieldID"].ToString();
                    field.DisplayType = fieldRow["DisplayType"].ToString();
                    field.DefaultGridCodeID = fieldRow["DefaultGridCodeID"].ToString();
                    field.DefaultGridText = fieldRow["DefaultGridText"].ToString();
                    field.UserGridFieldID = fieldRow["UserGridFieldID"].ToString();
                    gridFields.Add(field);
                }
                var gridCodes = new List<CommonBaseGridUserControl.UIGridCode>();
                foreach (DataRow codeRow in codeTable.Rows)
                {
                    var code = new CommonBaseGridUserControl.UIGridCode();
                    code.ID = codeRow["GridCodeID"].ToString();
                    code.IsAuthorized = true;
                    code.Code = codeRow["Code"].ToString();
                    code.Literal = codeRow["Literal"].ToString();
                    code.EffectiveDate = DataAccessHelper.ConvertToDateTime(codeRow["EffectiveDate"]);
                    code.ExpirationDate = DataAccessHelper.ConvertToDateTime(codeRow["ExpirationDate"]);
                    code.Disable = !DataAccessHelper.ConvertToBool(codeRow["ActiveIndicator"]);
                    code.GridFieldID = codeRow["GridFieldID"].ToString();
                    gridCodes.Add(code);
                }
                userGridFieldCodes.UserGridFields = gridFields;
                userGridFieldCodes.UserGridCodes = gridCodes;
            }

            return userGridFieldCodes;
        }
        public UserPreference GetUserPreference(string userId)
        {
            UserPreference userPref = null;
            try
            {
                DataSet ds = CartDAO.Instance.GetUserPreference(userId);
                userPref = GetUserPreferenceFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new CartGridLoadFailedException(ex.Message);
            }
            return userPref;
        }
        private UserPreference GetUserPreferenceFromDataSet(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count < 1)
            {
                return null;
            }
            var userPref = new UserPreference();
            DataRow row = ds.Tables[0].Rows[0];
            userPref.PrimaryCartId = DataAccessHelper.ConvertToString(row["PrimaryBasketID"]);
            userPref.DefaultGridTemplateId = DataAccessHelper.ConvertToString(row["DefaultGridTemplateID"]);
            userPref.Id = DataAccessHelper.ConvertToString(row["u_user_id"]);
            userPref.DefaultQuantity = DataAccessHelper.ConvertToInt(row["DefaultQuantity"]);
            return userPref;
        }
    }
}
