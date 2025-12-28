using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Cache;
using BT.TS360API.Common.Models;
using System.Data;
using System.Data.SqlClient;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Exceptions;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using Microsoft.SqlServer.Server;
using BT.TS360API.Common.Grid.Cart;

namespace BT.TS360API.Common.DataAccess
{
    public class GridDAOManager
    {
        private GridDAOManager()
        { }

        private static volatile GridDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static GridDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new GridDAOManager();
                }

                return _instance;
            }
        }

        private static readonly string USER_GRID_FIELDS_CACHE_KEY = "USER_GRID_FIELDS_CACHE_KEY_{0}";
        private static readonly string USER_OBJECT_CACHE_KEY = "USER_OBJECT_CACHE_KEY_{0}";

        public string GetDefaultGridTemplateId(string userId, string cartId)
        {
            return GridDAO.Instance.GetDefaultGridTemplateId(userId, cartId);
        }

        public List<CommonGridTemplateLine> LoadGridTemplateLines(string gridTemplateId, string userId, string orgId, bool isAuthorizedtoUseAllGridCodes)
        {
            var gridTemplateLines = GetGridTemplateLinesNew(gridTemplateId);

            CompileGridTemplateLineData(gridTemplateLines, userId, orgId, isAuthorizedtoUseAllGridCodes);
            gridTemplateLines = gridTemplateLines.OrderBy(x => x.Sequence).ToList();
            return gridTemplateLines;
        }

        private void CompileGridTemplateLineData(IEnumerable<CommonGridTemplateLine> gridTemplateLines, string _userId, string _orgId, bool isAuthorizedtoUseAllGridCodes)
        {
//            var activeGridFields = DistributedCacheHelper.GetActiveGridFieldsForOrg(_orgId, true);
//
//            //var userGridFieldsCodes = UserGridFieldsCodesManager.Instance.GetUserGridFieldsCodes(_userId, _orgId);
//            var userGridFieldsCodes = GetUserGridFieldsCodes(_userId, _orgId, isAuthorizedtoUseAllGridCodes);
//
//            if (gridTemplateLines != null)
//            {
//                foreach (var gridLine in gridTemplateLines)
//                {
//                    var gridCodeList = new List<GridTemplateFieldCode>();
//                    foreach (var gf in activeGridFields)
//                    {
//                        var isAdded = false;
//                        var newFc = new GridTemplateFieldCode();
//                        newFc.IsFreeText = gf.IsFreeText;
//                        newFc.GridFieldId = gf.ID;
//                        newFc.IsAuthorized = true;
//                        newFc.GridFieldType = (GridFieldType)Enum.Parse(typeof(GridFieldType), gf.GridFieldType);
//                        if (!gf.IsFreeText)
//                        {
//                            var isShowCode = true;
//                            if (userGridFieldsCodes != null && userGridFieldsCodes.UserGridFields != null)
//                            {
//                                var userGridField = userGridFieldsCodes.UserGridFields.FirstOrDefault(x => x.GridFieldID == gf.ID);
//                                if (userGridField != null)
//                                    isShowCode = string.IsNullOrEmpty(userGridField.DisplayType) ||
//                                                         userGridField.DisplayType.ToUpper() != "LITERAL";
//                            }
//                            if (userGridFieldsCodes != null)
//                            {
//                                isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.AgencyCodeID, gf.ID, GridFieldType.AgencyCode,
//                                    gf.UIGridCodes, userGridFieldsCodes.UserGridCodes, isShowCode);
//                                if (!isAdded)
//                                    isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.ItemTypeID, gf.ID, GridFieldType.ItemType, gf.UIGridCodes,
//                                        userGridFieldsCodes.UserGridCodes, isShowCode);
//                                if (!isAdded)
//                                    isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.CollectionID, gf.ID, GridFieldType.Collection, gf.UIGridCodes,
//                                        userGridFieldsCodes.UserGridCodes, isShowCode);
//                                if (!isAdded)
//                                    isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.UserCode1ID, gf.ID, GridFieldType.UserCode1, gf.UIGridCodes,
//                                        userGridFieldsCodes.UserGridCodes, isShowCode);
//                                if (!isAdded)
//                                    isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.UserCode2ID, gf.ID, GridFieldType.UserCode2, gf.UIGridCodes,
//                                        userGridFieldsCodes.UserGridCodes, isShowCode);
//                                if (!isAdded)
//                                    isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.UserCode3ID, gf.ID, GridFieldType.UserCode3, gf.UIGridCodes,
//                                        userGridFieldsCodes.UserGridCodes, isShowCode);
//                                if (!isAdded)
//                                    isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.UserCode4ID, gf.ID, GridFieldType.UserCode4, gf.UIGridCodes,
//                                        userGridFieldsCodes.UserGridCodes, isShowCode);
//                                if (!isAdded)
//                                    isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.UserCode5ID, gf.ID, GridFieldType.UserCode5, gf.UIGridCodes,
//                                        userGridFieldsCodes.UserGridCodes, isShowCode);
//                                if (!isAdded)
//                                    isAdded = AddFieldCodeInTemplate(gridCodeList, gridLine.UserCode6ID, gf.ID, GridFieldType.UserCode6, gf.UIGridCodes,
//                                        userGridFieldsCodes.UserGridCodes, isShowCode);
//                            }
//                        }
//                        else
//                        {
//                            var orgFieldCode = GridDataHelper.LookUpCallNumberGridFieldCodeInOrg(activeGridFields);
//                            if (orgFieldCode != null && orgFieldCode.GridFieldId == gf.ID)
//                            {
//                                var callNumberFieldCode = new GridTemplateFieldCode
//                                {
//                                    GridFieldId = orgFieldCode.GridFieldId,
//                                    GridText = gridLine.CallNumberText,
//                                    IsAuthorized = true,
//                                    IsExpired = false,
//                                    IsFreeText = true,
//                                    GridFieldType = GridFieldType.CallNumber
//                                };
//                                gridCodeList.Add(callNumberFieldCode);
//                                isAdded = true;
//                            }
//                        }
//                        if (!isAdded)
//                            gridCodeList.Add(newFc);
//                    }
//                    gridLine.GridFieldCodeList = gridCodeList;
//                }
//            }
        }

        private bool AddFieldCodeInTemplate(List<GridTemplateFieldCode> gridCodeList, string gridCodeId, string gridFieldId, GridFieldType gridFieldType,
            List<CommonBaseGridUserControl.UIGridCode> orgGridCodes,
            List<CommonBaseGridUserControl.UIGridCode> userGridCodes, bool isShowCode = true)
        {
            var fc = GridDataHelper.LookUpGridFieldCode(userGridCodes, gridCodeId, isShowCode);
            if (fc != null)
            {
                fc.IsAuthorized = true;
            }
            else
            {
                fc = GridDataHelper.LookUpGridFieldCode(orgGridCodes, gridCodeId, isShowCode);
                if (fc != null)
                {
                    fc.IsAuthorized = false;
                }
            }
            if (fc != null && fc.GridFieldId == gridFieldId)
            {
                gridCodeList.Add(new GridTemplateFieldCode()
                {
                    GridCodeId = fc.GridCodeId,
                    GridFieldId = fc.GridFieldId,
                    GridCode = fc.GridCodeValue,
                    GridText = fc.GridTextValue,
                    IsExpired = fc.IsExpired,
                    IsFutureDate = fc.IsFutureDate,
                    IsDisabled = fc.IsDisabled,
                    IsAuthorized = fc.IsAuthorized,
                    IsExpiredOrFutureDate = fc.IsExpiredOrFutureDate,
                    GridFieldType = gridFieldType
                });
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <param name="isAuthorizedtoUseAllGridCodes">SiteContext.Current.IsAuthorizedtoUseAllGridCodes</param>
        /// <returns></returns>
        public UserGridFieldsCodes GetUserGridFieldsCodes(string userId, string orgId, bool isAuthorizedtoUseAllGridCodes)
        {
            var userGridFieldCodes = new UserGridFieldsCodes();

            //userGridFieldCodes = GetUserGridFieldsCodesFromAppCache(userId, orgId);
            //if (userGridFieldCodes != null) return userGridFieldCodes;

            //userGridFieldCodes = new UserGridFieldsCodes();

            //if (CommonHelper.IsAuthorizeToUseAllGridCodes(orgId))
            if (isAuthorizedtoUseAllGridCodes)
            {
                var gridCodes = new List<CommonBaseGridUserControl.UIGridCode>();
                var orgGridFieldCodes = DistributedCacheHelper.GetActiveGridFieldsForOrg(orgId, true);
                foreach (var gf in orgGridFieldCodes)
                {
                    gridCodes.AddRange(gf.UIGridCodes);
                }

                gridCodes.ForEach(item => item.IsAuthorized = true);

                userGridFieldCodes.UserGridCodes = gridCodes;
                userGridFieldCodes.UserGridFields = GetUserGridFields(userId);
                userGridFieldCodes.DefaultQuantity = GetDefaultQuantity(userId);
            }
            else
            {
                userGridFieldCodes = GetUserGridFieldsCodes(userId, orgId);
            }

            //StoreUserGridFieldsCodesToAppCache(userId, orgId, userGridFieldCodes);

            return userGridFieldCodes;
        }

        public UserGridFieldsCodes GetUserGridFieldsCodes(string userId, string orgId)
        {
            var defaultQuantity = 0;

            //var ds = _gridDaoManager.GetUserGridFieldsCodes(userId, orgId, out defaultQuantity);
            var ds = GridDAO.Instance.GetUserGridFieldsCodes(userId, orgId, out defaultQuantity);

            var userGridFieldCodes = RefineUserGridFieldsCodes(ds);
            userGridFieldCodes.DefaultQuantity = defaultQuantity;

            return userGridFieldCodes;
        }
        public DataSet GetUserGridFieldsCodes(string userId, string orgId, out int defaultQuantity)
        {
            return GridDAO.Instance.GetUserGridFieldsCodes(userId, orgId, out defaultQuantity);
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

        private List<CommonBaseGridUserControl.UIUserGridField> GetUserGridFields(string userId)
        {
            var cacheKey = string.Format(USER_GRID_FIELDS_CACHE_KEY, userId);
            
            //var userGridFields = VelocityCacheManager.Read(cacheKey) as List<CommonBaseGridUserControl.UIUserGridField>;
            var userGridFields = CachingController.Instance.Read(cacheKey) as List<CommonBaseGridUserControl.UIUserGridField>;

            if (userGridFields == null)
            {
                userGridFields = GetGridFieldsByUser(userId);

                //VelocityCacheManager.Write(cacheKey, userGridFields, VelocityCacheLevel.Session);
                CachingController.Instance.Write(cacheKey, userGridFields);
            }
            return userGridFields;
        }

        public List<CommonBaseGridUserControl.UIUserGridField> GetGridFieldsByUser(string userId)
        {
            var gridFields = new List<CommonBaseGridUserControl.UIUserGridField>();
            //var ds = _gridDaoManager.GetGridFieldsByUser(userId);
            var ds = GridDAO.Instance.GetGridFieldsByUser(userId);
            if (ds != null && ds.Tables.Count > 0)
            {
                var fieldTable = ds.Tables[0];
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
            }
            return gridFields;
        }

        private int GetDefaultQuantity(string userId)
        {
            var cacheKey = string.Format(USER_OBJECT_CACHE_KEY, userId);

            //var userObj = VelocityCacheManager.Read(cacheKey, VelocityCacheLevel.Session) as UserPreference;
            var userObj = CachingController.Instance.Read(cacheKey) as UserPreference;

            if (userObj == null)
            {
                //userObj = GetUserPreference();
                userObj = GetUserPreference(userId);

                //VelocityCacheManager.Write(cacheKey, userObj, VelocityCacheLevel.Session);
                CachingController.Instance.Write(cacheKey, userObj);
            }
            if (userObj != null)
                return userObj.DefaultQuantity;

            return 0;
        }

        public UserPreference GetUserPreference(string userId)
        {
            UserPreference userPref = null;

            //DataSet ds = _gridDaoManager.GetUserPreference();
            DataSet ds = GridDAO.Instance.GetUserPreference(userId);

            userPref = GetUserPreferenceFromDataSet(ds);

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
            //userPref.BeginDataLoading();
            userPref.PrimaryCartId = DataAccessHelper.ConvertToString(row["PrimaryBasketID"]);
            userPref.DefaultGridTemplateId = DataAccessHelper.ConvertToString(row["DefaultGridTemplateID"]);
            userPref.Id = DataAccessHelper.ConvertToString(row["u_user_id"]);
            userPref.DefaultQuantity = DataAccessHelper.ConvertToInt(row["DefaultQuantity"]);
            //userPref.EndDataLoading();
            return userPref;
        }

        public List<CommonGridTemplateLine> GetGridTemplateLinesNew(string gridTemplateId)
        {
            var gridTemplateLines = new List<CommonGridTemplateLine>();
            var ds = GridDAO.Instance.GetGridTemplateLines(gridTemplateId);
            gridTemplateLines = GetGridTemplateLinesFromDataSet(ds);
            return gridTemplateLines;
        }

        private List<CommonGridTemplateLine> GetGridTemplateLinesFromDataSet(DataSet ds)
        {
            var results = new List<CommonGridTemplateLine>();
            if (ds != null && ds.Tables.Count > 0)
            {
                var gridTmplTable = ds.Tables[0];
                foreach (DataRow row in gridTmplTable.Rows)
                {
                    var line = new CommonGridTemplateLine
                    {
                        ID = row["GridTemplateLineID"].ToString(),
                        GridTemplateID = row["GridTemplateID"].ToString(),
                        AgencyCodeID = row["AgencyCodeID"].ToString(),
                        ItemTypeID = row["ItemTypeID"].ToString(),
                        CollectionID = row["CollectionID"].ToString(),
                        CallNumberText = row["CallNumberText"].ToString(),
                        UserCode1ID = row["UserCode1ID"].ToString(),
                        UserCode2ID = row["UserCode2ID"].ToString(),
                        UserCode3ID = row["UserCode3ID"].ToString(),
                        UserCode4ID = row["UserCode4ID"].ToString(),
                        UserCode5ID = row["UserCode5ID"].ToString(),
                        UserCode6ID = row["UserCode6ID"].ToString(),
                        Qty = DataAccessHelper.ConvertToInt(row["Quantity"]),
                        Sequence = DataAccessHelper.ConvertToInt(row["Sequence"]),
                        EnabledIndicator = DataAccessHelper.ConvertToBool(row["EnabledIndicator"]),
                        //
                        IsTempDisabled = DataAccessHelper.ConvertToBool(row["TempDisabledIndicator"]),
                        //
                        CreatedBy = row["CreatedBy"].ToString(),
                        UpdatedBy = row["UpdatedBy"].ToString()
                    };

                    var createdDateTime = DataAccessHelper.ConvertToDateTime(row["CreatedDateTime"]);
                    if (createdDateTime != null)
                    {
                        line.CreatedDateTime = createdDateTime.Value;
                    }

                    createdDateTime = DataAccessHelper.ConvertToDateTime(row["UpdatedDateTime"]);
                    if (createdDateTime != null)
                    {
                        line.UpdatedDateTime = createdDateTime.Value;
                    }   
                    results.Add(line);
                }
            }

            return results;
        }
        public async Task<List<NoteClientObject>> GetNotesByBTKeysAsync(string cartId, string userId, List<string> btkeys, List<string> lineItemIds = null)
        {
            DataSet ds = await GridDAO.Instance.GetNotesByBTKeysAsync(cartId, userId, btkeys, lineItemIds);
            return GetNotesByBTKeysFromDataSet(ds);
        }
        private List<NoteClientObject> GetNotesByBTKeysFromDataSet(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return null;
            var notes = new List<NoteClientObject>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var note = new NoteClientObject();
                var btkey = DataAccessHelper.ConvertToString(row["BTkey"]);
                if (string.IsNullOrEmpty(btkey))
                {
                    btkey = DataAccessHelper.ConvertToString(row["BasketOriginalEntryID"]);
                    //if (!string.IsNullOrEmpty(btkey))
                    //{
                    //    btkey = btkey.Replace("{", "").Replace("}", "");
                    //}
                }
                note.BTKey = btkey;
                note.LineItemId = DataAccessHelper.ConvertToString(row["BasketLineItemID"]);
                note.NotesCount = DataAccessHelper.ConvertToString(row["NotesCount"]);
                note.Note = DataAccessHelper.ConvertToString(row["Note"]);
                note.MyQty = DataAccessHelper.ConvertToString(row["Quantity"]);
                if (string.IsNullOrEmpty(note.MyQty))
                    note.MyQty = DataAccessHelper.ConvertToString(row["NoteQuantity"]);

                notes.Add(note);
            }
            return notes;
        }

        public List<GridNoteCount> GetCountForLineItem(string cartId, List<string> lineItems)
        {
            DataSet ds = GridDAO.Instance.GetCountForLineItems(cartId, lineItems);
            return GetGridNoteCountLineItemsFromDataSet(ds);
        }

        private List<GridNoteCount> GetGridNoteCountLineItemsFromDataSet(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return null;
            var gridNoteCounts = new List<GridNoteCount>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var gridNote = new GridNoteCount();
                gridNote.LineItemId = DataAccessHelper.ConvertToString(row["BasketLineItemId"]);
                //gridNote.NoteCount = DataAccessHelper.ConvertToInt(row["CountUsersNotes"]);
                gridNote.QuantityCount = DataAccessHelper.ConvertToInt(row["CountUsersQuantity"]);
                gridNote.GridLineCount = DataAccessHelper.ConvertToInt(row["CountGridLines"]);
                gridNote.LineItemTotalQuantity = DataAccessHelper.ConvertToInt(row["LineItemTotalQuantity"]);
                gridNoteCounts.Add(gridNote);
            }
            return gridNoteCounts;
        }
        public void SaveUserGridFieldsCodes(string userId, List<CommonBaseGridUserControl.UIUserGridField> userGridFieldObjects, int defaultQuantity)
        {
            var sqlDataRecords = DataConverter.ConvertUIUserGridFieldToDataSet(userGridFieldObjects);
            GridDAO.Instance.SaveUserGridFieldsCodes(userId, sqlDataRecords, defaultQuantity);
        }
        public Dictionary<string, List<CommonCartGridLine>> GetCartGridLinesNewGrid(List<string> lineItemIds)
        {
            Dictionary<string, List<CommonCartGridLine>> lines = null;
            try
            {
                DataSet ds = GridDAO.Instance.GetCartGridLines(lineItemIds);
                lines = CartGridDataAccessManager.Instance.GetCartGridLinesFromDataSetNewGrid(ds);
            }
            catch (Exception cartGridDatabaseException)
            {
                throw new CartGridLoadFailedException(cartGridDatabaseException.Message);
            }
            return lines;
        }
    }
}
