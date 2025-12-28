using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Cache;
using BT.TS360API.Common.Configrations;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Models;
using BT.TS360Constants;

namespace BT.TS360API.Common.Helpers
{
    public static class DistributedCacheHelper
    {

        public static Dictionary<string, int> GetDemandBucket()
        {
            const string cacheKey = "__DemandBucketSettingsCacheKey";
            // var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as Dictionary<string, int>;
            var cache = CachingController.Instance.Read(cacheKey) as Dictionary<string, int>;
            if (cache == null)
            {
                cache = DemandBucketSettings.GetSettings();
                //VelocityCacheManager.Write(cacheKey, cache, CommonCacheContant.Ts360FarmCacheName);
                CachingController.Instance.Write(cacheKey, cache);
            }

            return cache;
        }

        public static string GetCurrentUserReservedType(string userId = null)
        {
            var reservedType = string.Empty;
            if (string.IsNullOrEmpty(userId))
                userId = ServiceRequestContext.Current.UserId;

            if (!string.IsNullOrEmpty(userId))
        {
                var cachekey = string.Format(DistributedCacheKey.CurrentUserInventoryType, userId.ToLower());
                var result = CachingController.Instance.Read(cachekey) as string;
                if (result != null)
                    reservedType = result;
            }

            return reservedType;
        }

        public static List<CommonBaseGridUserControl.UIGridField> GetActiveGridFieldsForOrg(string orgId, bool hasGridCodes = false)
        {
            //List<CommonBaseGridUserControl.UIGridField> listFieldsCodes = GetActiveGridFieldsForOrgFromAppCache(orgId, hasGridCodes);
            //if (listFieldsCodes != null)
            //{
            //    return listFieldsCodes;
            //}

            List<CommonBaseGridUserControl.UIGridField> listFieldsCodes = null;

            var cacheKey = GetGridFieldCodeCacheKey(orgId);

            //var fieldCodeDs = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as DataSet;
            var fieldCodeDs = CachingController.Instance.Read(cacheKey) as DataSet;

            if (fieldCodeDs == null)
            {
                fieldCodeDs = GetGridFieldsCodesForOrgFromDao(orgId);
                //VelocityCacheManager.Write(cacheKey, fieldCodeDs, CommonCacheContant.Ts360FarmCacheName);
                CachingController.Instance.Write(cacheKey, fieldCodeDs);
            }

            listFieldsCodes = RefineDsAndGetActiveUiGridField(fieldCodeDs, hasGridCodes);

            //StoreActiveGridFieldsForOrgToAppCache(orgId, listFieldsCodes, hasGridCodes);
            return listFieldsCodes;
        }

        public static DataSet GetGridFieldsCodesForOrgFromDao(string orgId)
        {
            var ds = OrdersDAO.Instance.GetGridFieldsCodesForOrg(orgId);
            return ds;
        }

        public static List<CommonBaseGridUserControl.UIGridField> RefineDsAndGetActiveUiGridField(DataSet dsFieldCode, bool hasGridCodes = false)
        {
            var results = new List<CommonBaseGridUserControl.UIGridField>();
            if (dsFieldCode != null && dsFieldCode.Tables.Count == 2)
            {
                var fieldTable = dsFieldCode.Tables[0];
                var codeTable = dsFieldCode.Tables[1];
                var sequence = 1;
                foreach (DataRow fieldRow in fieldTable.Rows)
                {
                    //Only return Active Grid Field
                    var isActive = DataAccessHelper.ConvertToBool(fieldRow["ActiveIndicator"]);
                    if (!isActive) continue;

                    var gridFieldType = fieldRow["GridFieldType"].ToString();
                    if (gridFieldType == "Unused") continue;

                    var gridFieldID = fieldRow["GridFieldID"].ToString();
                    var field = new CommonBaseGridUserControl.UIGridField();
                    field.ID = gridFieldID;
                    field.GridFieldType = gridFieldType;
                    field.Name = fieldRow["Name"].ToString();
                    field.Sequence = sequence;
                    field.ActiveIndicator = true;
                    field.CreatedBy = fieldRow["CreatedBy"].ToString();
                    field.CreatedDateTime = DataAccessHelper.ConvertToDateTime(fieldRow["CreatedDateTime"]);
                    field.UpdatedBy = fieldRow["UpdatedBy"].ToString();
                    field.UpdatedDateTime = DataAccessHelper.ConvertToDateTime(fieldRow["UpdatedDateTime"]);
                    field.LastESPVerificationDateString = DataAccessHelper.ConvertToDateTime(fieldRow["LastESPVerificationDate"]).HasValue ?
                                                        Convert.ToDateTime(fieldRow["LastESPVerificationDate"]).ToString("MM/dd/yyyy hh:mm tt") : "";
                    field.IsESPFundCodeField = DataAccessHelper.ConvertToBool(fieldRow["IsESPFundCodeField"]);
                    field.IsESPBranchCodeField = DataAccessHelper.ConvertToBool(fieldRow["IsESPBranchCodeField"]);
                    if (hasGridCodes)
                    {
                        field.UIGridCodes = new List<CommonBaseGridUserControl.UIGridCode>();
                        var listCodeRows = codeTable.Select(string.Format("GridFieldID='{0}'", gridFieldID));
                        if (listCodeRows.Length > 0)
                        {
                            foreach (DataRow codeRow in listCodeRows)
                            {
                                var code = new CommonBaseGridUserControl.UIGridCode();
                                code.ID = codeRow["GridCodeID"].ToString();
                                code.Literal = codeRow["Literal"].ToString();
                                code.EffectiveDate = DataAccessHelper.ConvertToDateTime(codeRow["EffectiveDate"]);
                                code.ExpirationDate = DataAccessHelper.ConvertToDateTime(codeRow["ExpirationDate"]);
                                code.Disable = !DataAccessHelper.ConvertToBool(codeRow["ActiveIndicator"]);
                                code.Delete = false;
                                code.Code = codeRow["Code"].ToString();
                                code.UserCount = DataAccessHelper.ConvertToInt(codeRow["UserCount"]);
                                code.GridFieldID = codeRow["GridFieldID"].ToString();
                                field.UIGridCodes.Add(code);
                            }
                        }
                    }
                    else
                    {
                        field.UIGridCodes = null;
                    }
                    sequence += 1;
                    results.Add(field);
                }
            }

            return SortGridFields(results);
        }

        public static List<CommonBaseGridUserControl.UIGridField> SortGridFields(List<CommonBaseGridUserControl.UIGridField> gridFields)
        {
            var tempGridFields = new CommonBaseGridUserControl.UIGridField[10];

            foreach (var gridField in gridFields)
            {
                var gridFieldType = CommonHelper.ConvertToGridFieldType(gridField.GridFieldType);
                var position = (int)gridFieldType;
                if (position >= 0 && tempGridFields[position] == null)
                {
                    tempGridFields[position] = gridField;
                }
            }

            return tempGridFields.Where(gf => gf != null).ToList();
        }

        //private static List<CommonBaseGridUserControl.UIGridField> RefineDsToListUiGridField(DataSet dsFieldCode)
        //{
        //    var results = new List<CommonBaseGridUserControl.UIGridField>();
        //    if (dsFieldCode == null || dsFieldCode.Tables.Count != 2) return results;

        //    var fieldTable = dsFieldCode.Tables[0];
        //    var codeTable = dsFieldCode.Tables[1];
        //    var sequence = 1;
        //    foreach (DataRow fieldRow in fieldTable.Rows)
        //    {
        //        var gridFieldType = fieldRow["GridFieldType"].ToString();
        //        var fieldName = fieldRow["Name"].ToString() == gridFieldType ? "" : fieldRow["Name"].ToString();

        //        var gridFieldId = fieldRow["GridFieldID"].ToString();

        //        var field = new CommonBaseGridUserControl.UIGridField();
        //        field.ID = gridFieldId;
        //        field.GridFieldType = gridFieldType;
        //        field.Name = fieldName;
        //        field.Sequence = sequence;
        //        field.SlipReportSequence = DataAccessHelper.ConvertToInt(fieldRow["SlipReportSequence"]);
        //        field.ActiveIndicator = DataAccessHelper.ConvertToBool(fieldRow["ActiveIndicator"]);
        //        field.CreatedBy = fieldRow["CreatedBy"].ToString();
        //        field.CreatedDateTime = DataAccessHelper.ConvertToDateTime(fieldRow["CreatedDateTime"]);
        //        field.UpdatedBy = fieldRow["UpdatedBy"].ToString();
        //        field.UpdatedDateTime = DataAccessHelper.ConvertToDateTime(fieldRow["UpdatedDateTime"]);
        //        field.LastESPVerificationDateString = DataAccessHelper.ConvertToDateTime(fieldRow["LastESPVerificationDate"]).HasValue ?
        //                                                Convert.ToDateTime(fieldRow["LastESPVerificationDate"]).ToString("MM/dd/yyyy hh:mm tt") : "";
        //        field.IsESPFundCodeField = DataAccessHelper.ConvertToBool(fieldRow["IsESPFundCodeField"]);
        //        field.IsESPBranchCodeField = DataAccessHelper.ConvertToBool(fieldRow["IsESPBranchCodeField"]);

        //        field.UIGridCodes = new List<CommonBaseGridUserControl.UIGridCode>();
        //        var listCodeRows = codeTable.Select(string.Format("GridFieldID='{0}'", gridFieldId));
        //        if (listCodeRows.Length > 0)
        //        {
        //            foreach (DataRow codeRow in listCodeRows)
        //            {
        //                var code = new CommonBaseGridUserControl.UIGridCode();
        //                code.ID = codeRow["GridCodeID"].ToString();
        //                code.Literal = codeRow["Literal"].ToString();
        //                code.EffectiveDate = DataAccessHelper.ConvertToDateTime(codeRow["EffectiveDate"]);
        //                code.ExpirationDate = DataAccessHelper.ConvertToDateTime(codeRow["ExpirationDate"]);
        //                code.Disable = !DataAccessHelper.ConvertToBool(codeRow["ActiveIndicator"]);
        //                code.Delete = false;
        //                code.Code = codeRow["Code"].ToString();
        //                code.UserCount = DataAccessHelper.ConvertToInt(codeRow["UserCount"]);

        //                field.UIGridCodes.Add(code);
        //            }
        //        }

        //        sequence += 1;
        //        results.Add(field);
        //    }

        //    return results;
        //}

        //private static List<CommonBaseGridUserControl.UIGridField> GetActiveGridFieldsForOrgFromAppCache(string orgId, bool hasGridCodes = false)
        //{
        //    var cacheKey = string.Format("OrgGridFieldsCacheKey_{0}_{1}", orgId, hasGridCodes);

        //    return VelocityCacheManager.Read(cacheKey, VelocityCacheLevel.Request) as List<UIGridField>;
        //}

        private static string GetGridFieldCodeCacheKey(string orgId)
        {
            return string.Format(DistributedCacheKey.GridFieldsCodesCacheKey, orgId);
        }


        /*
                public static bool VerifyGridFieldsCodesCacheData(string orgId)
                {
                    var cacheKey = GetGridFieldCodeCacheKey(orgId);
                    return HasCacheData(cacheKey);
                }
                public static List<UIGridField> GetGridFieldsCodesForOrg(string orgId)
                {
                    var cacheKey = GetGridFieldCodeCacheKey(orgId);
                    List<UIGridField> listFieldsCodes;

                    var fieldCodeDs = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as DataSet;

                    if (fieldCodeDs != null)
                    {
                        listFieldsCodes = RefineDsToListUiGridField(fieldCodeDs);
                        return listFieldsCodes;
                    }

                    fieldCodeDs = GetGridFieldsCodesForOrgFromDao(orgId);
                    VelocityCacheManager.Write(cacheKey, fieldCodeDs, CommonCacheContant.Ts360FarmCacheName);

                    listFieldsCodes = RefineDsToListUiGridField(fieldCodeDs);
                    return listFieldsCodes;
                }

        

        

                private static void StoreActiveGridFieldsForOrgToAppCache(string orgId, List<UIGridField> gridFields, bool hasGridCodes = false)
                {
                    var cacheKey = string.Format("OrgGridFieldsCacheKey_{0}_{1}", orgId, hasGridCodes);

                    VelocityCacheManager.Write(cacheKey, gridFields, VelocityCacheLevel.Request);
                }

                public static bool VerifyGridTemplatesCacheData(string orgId)
                {
                    var cacheKey = GetGridTemplatesCacheKey(orgId);
                    return HasCacheData(cacheKey);
                }

                public static List<CommonGridTemplate> GetGridTemplatesForOrg(string orgId)
                {
                    if (string.IsNullOrEmpty(orgId))
                        return null;
                    var cacheKey = GetGridTemplatesCacheKey(orgId);
                    List<CommonGridTemplate> listTpls;

                    var dsTemplates = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as
                        DataSet;

                    if (dsTemplates != null)
                    {
                        listTpls = RefineDsToGridTemplates(dsTemplates);
                        return listTpls;
                    }

                    dsTemplates = GetGridTemplatesForOrgFromDao(orgId);
                    VelocityCacheManager.Write(cacheKey, dsTemplates, CommonCacheContant.Ts360FarmCacheName);

                    listTpls = RefineDsToGridTemplates(dsTemplates);
                    return listTpls;
                }

                public static List<CommonGridTemplate> GetGridTemplatesByOwner(string onwerId, string orgId, bool hasDefaultGridTemplate = false)
                {
                    if (string.IsNullOrEmpty(orgId))
                        return null;
                    var cacheKey = GetGridTemplatesCacheKey(orgId);
                    List<CommonGridTemplate> listTpls;

                    var dsTemplates = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as
                        DataSet;

                    if (dsTemplates == null)
                    {
                        dsTemplates = GetGridTemplatesForOrgFromDao(orgId);
                        VelocityCacheManager.Write(cacheKey, dsTemplates, CommonCacheContant.Ts360FarmCacheName);
                    }

                    listTpls = RefineDsToGridTemplates(dsTemplates);
                    if (listTpls != null)
                    {
                        if (!hasDefaultGridTemplate)
                            return
                                listTpls.Where(x => x.OwnerUserID == onwerId && string.IsNullOrEmpty(x.DefaultBasketSummaryID))
                                .ToList();
                        return listTpls.Where(x => x.OwnerUserID == onwerId).ToList();
                    }

                    return null;
                }

                public static CommonGridTemplate GetGridTemplate(string templateId, string orgId)
                {
                    if (string.IsNullOrEmpty(orgId))
                        return null;
                    var cacheKey = GetGridTemplatesCacheKey(orgId);

                    var dsTemplates = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as
                        DataSet;

                    if (dsTemplates == null)
                    {
                        dsTemplates = GetGridTemplatesForOrgFromDao(orgId);
                        VelocityCacheManager.Write(cacheKey, dsTemplates, CommonCacheContant.Ts360FarmCacheName);
                    }

                    var listTpls = RefineDsToGridTemplates(dsTemplates);
                    if (listTpls != null && listTpls.Count > 0)
                        return listTpls.FirstOrDefault(x => x.GridTemplateId == templateId);
                    return null;
                }

                public static CommonGridTemplate GetDefaultGridTemplate(string userId, string orgId, string cartId)
                {
                    var userGridTemplates = GetGridTemplatesByOwner(userId, orgId, true);
                    if (userGridTemplates != null)
                    {
                        return userGridTemplates.FirstOrDefault(x => x.DefaultBasketSummaryID == cartId);
                    }
                    return null;
                }

                public static void SetExpiredGridFieldsCodes(string orgId)
                {
                    var cacheKey = GetGridFieldCodeCacheKey(orgId);
                    VelocityCacheManager.SetExpired(cacheKey, CommonCacheContant.Ts360FarmCacheName);
                }

                public static void SetExpiredGridTemplates(string orgId)
                {
                    var cacheKey = GetGridTemplatesCacheKey(orgId);
                    VelocityCacheManager.SetExpired(cacheKey, CommonCacheContant.Ts360FarmCacheName);
                }

                public static void PopulateGridFieldsCodesCacheData(string orgId)
                {
                    var hasFieldCodeCache = VerifyGridFieldsCodesCacheData(orgId);
                    if (!hasFieldCodeCache)
                    {
                        GetGridFieldsCodesForOrg(orgId);
                    }
                }

                public static void PopulateGridTemplatesCacheData(string orgId)
                {
                    var hasGridTemplatesCache = VerifyGridTemplatesCacheData(orgId);
                    if (!hasGridTemplatesCache)
                    {
                        GetGridTemplatesForOrg(orgId);
                    }
                }

                public static void UpdateGridFieldsCodesCacheData(string orgId)
                {
                    SetExpiredGridFieldsCodes(orgId);
                    GetGridFieldsCodesForOrg(orgId);
                }

                public static void UpdateGridTemplatesCacheData(string orgId)
                {
                    SetExpiredGridTemplates(orgId);
                    GetGridTemplatesForOrg(orgId);
                }

                #region Private
                private static bool HasCacheData(string cacheKey)
                {
                    var cacheObject = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName);
                    return cacheObject != null;
                }

        

                private static List<UIGridField> RefineDsAndGetActiveUiGridField(DataSet dsFieldCode, bool hasGridCodes = false)
                {
                    var results = new List<UIGridField>();
                    if (dsFieldCode != null && dsFieldCode.Tables.Count == 2)
                    {
                        var fieldTable = dsFieldCode.Tables[0];
                        var codeTable = dsFieldCode.Tables[1];
                        var sequence = 1;
                        foreach (DataRow fieldRow in fieldTable.Rows)
                        {
                            //Only return Active Grid Field
                            var isActive = DataAccessHelper.ConvertToBool(fieldRow["ActiveIndicator"]);
                            if (!isActive) continue;

                            var gridFieldType = fieldRow["GridFieldType"].ToString();
                            if (gridFieldType == "Unused") continue;

                            var gridFieldID = fieldRow["GridFieldID"].ToString();
                            var field = new UIGridField();
                            field.ID = gridFieldID;
                            field.GridFieldType = gridFieldType;
                            field.Name = fieldRow["Name"].ToString();
                            field.Sequence = sequence;
                            field.ActiveIndicator = true;
                            field.CreatedBy = fieldRow["CreatedBy"].ToString();
                            field.CreatedDateTime = DataAccessHelper.ConvertToDateTime(fieldRow["CreatedDateTime"]);
                            field.UpdatedBy = fieldRow["UpdatedBy"].ToString();
                            field.UpdatedDateTime = DataAccessHelper.ConvertToDateTime(fieldRow["UpdatedDateTime"]);
                            field.LastESPVerificationDateString = DataAccessHelper.ConvertToDateTime(fieldRow["LastESPVerificationDate"]).HasValue ?
                                                                Convert.ToDateTime(fieldRow["LastESPVerificationDate"]).ToString("MM/dd/yyyy hh:mm tt") : "";
                            field.IsESPFundCodeField = DataAccessHelper.ConvertToBool(fieldRow["IsESPFundCodeField"]);
                            field.IsESPBranchCodeField = DataAccessHelper.ConvertToBool(fieldRow["IsESPBranchCodeField"]);
                            if (hasGridCodes)
                            {
                                field.UIGridCodes = new List<CommonBaseGridUserControl.UIGridCode>();
                                var listCodeRows = codeTable.Select(string.Format("GridFieldID='{0}'", gridFieldID));
                                if (listCodeRows.Length > 0)
                                {
                                    foreach (DataRow codeRow in listCodeRows)
                                    {
                                        var code = new CommonBaseGridUserControl.UIGridCode();
                                        code.ID = codeRow["GridCodeID"].ToString();
                                        code.Literal = codeRow["Literal"].ToString();
                                        code.EffectiveDate = DataAccessHelper.ConvertToDateTime(codeRow["EffectiveDate"]);
                                        code.ExpirationDate = DataAccessHelper.ConvertToDateTime(codeRow["ExpirationDate"]);
                                        code.Disable = !DataAccessHelper.ConvertToBool(codeRow["ActiveIndicator"]);
                                        code.Delete = false;
                                        code.Code = codeRow["Code"].ToString();
                                        code.UserCount = DataAccessHelper.ConvertToInt(codeRow["UserCount"]);
                                        code.GridFieldID = codeRow["GridFieldID"].ToString();
                                        field.UIGridCodes.Add(code);
                                    }
                                }
                            }
                            else
                            {
                                field.UIGridCodes = null;
                            }
                            sequence += 1;
                            results.Add(field);
                        }
                    }

                    return SortGridFields(results);
                }

                private static List<CommonGridTemplate> RefineDsToGridTemplates(DataSet dsTemplates)
                {
                    if (dsTemplates == null || dsTemplates.Tables.Count == 0)
                    {
                        return new List<CommonGridTemplate>();
                    }

                    var results = new List<CommonGridTemplate>();
                    if (dsTemplates.Tables.Count > 0)
                    {
                        var tplTable = dsTemplates.Tables[0];
                        foreach (DataRow templateRow in tplTable.Rows)
                        {
                            var gridTemplate = new CommonGridTemplate();
                            gridTemplate.GridTemplateId = templateRow["GridTemplateID"].ToString();
                            gridTemplate.Name = templateRow["Name"].ToString();
                            gridTemplate.Description = templateRow["Description"].ToString();
                            gridTemplate.EnabledIndicator = DataAccessHelper.ConvertToBool(templateRow["EnabledIndicator"]);
                            gridTemplate.CreatedBy = templateRow["CreatedBy"].ToString();
                            var convertToDateTime = DataAccessHelper.ConvertToDateTime(templateRow["UpdatedDateTime"]);
                            if (convertToDateTime != null)
                                gridTemplate.LastModified = convertToDateTime.Value;
                            gridTemplate.DefaultBasketSummaryID = templateRow["DefaultBasketSummaryID"].ToString();
                            gridTemplate.OwnerUserID = templateRow["OwnerUserID"].ToString();
                            gridTemplate.OwnerUserName = templateRow["OwnerUserName"].ToString();
                            gridTemplate.NumberOfRows = DataAccessHelper.ConvertToInt(templateRow["LineCount"]);
                            gridTemplate.NumberOfUsers = DataAccessHelper.ConvertToInt(templateRow["UserCount"]);
                            gridTemplate.IsGridDistribution = DataAccessHelper.ConvertToBool(templateRow["IsGridDistribution"]);
                            gridTemplate.GridDistributionOption = DataAccessHelper.ConvertToIntNullable(templateRow["GridDistributionOptionID"]);

                            results.Add(gridTemplate);
                        }
                    }

                    return results;
                }

                private static List<CommonGridTemplateLine> RefineDsToGridTemplateLines(DataSet dsTemplateLines, string tplId)
                {
                    if (dsTemplateLines == null || dsTemplateLines.Tables.Count == 0)
                    {
                        return new List<CommonGridTemplateLine>();
                    }

                    var results = new List<CommonGridTemplateLine>();
                    if (dsTemplateLines != null && dsTemplateLines.Tables.Count == 2 && dsTemplateLines.Tables[1].Rows.Count > 0)
                    {
                        var linesTable = dsTemplateLines.Tables[1].Select(string.Format("GridTemplateID='{0}'", tplId));
                        foreach (DataRow lineRowRow in linesTable)
                        {
                            var line = new CommonGridTemplateLine
                            {
                                ID = lineRowRow["GridTemplateLineID"].ToString(),
                                AgencyCodeID = lineRowRow["AgencyCodeID"].ToString(),
                                ItemTypeID = lineRowRow["ItemTypeID"].ToString(),
                                CollectionID = lineRowRow["CollectionID"].ToString(),
                                CallNumberText = lineRowRow["CallNumberText"].ToString(),
                                UserCode1ID = lineRowRow["UserCode1ID"].ToString(),
                                UserCode2ID = lineRowRow["UserCode2ID"].ToString(),
                                UserCode3ID = lineRowRow["UserCode3ID"].ToString(),
                                UserCode4ID = lineRowRow["UserCode4ID"].ToString(),
                                UserCode5ID = lineRowRow["UserCode5ID"].ToString(),
                                UserCode6ID = lineRowRow["UserCode6ID"].ToString(),
                                Qty = DataAccessHelper.ConvertToInt(lineRowRow["Quantity"]),
                                Sequence =
                                    DataAccessHelper.ConvertToInt(lineRowRow["Sequence"]),
                                EnabledIndicator =
                                    DataAccessHelper.ConvertToBool(
                                        lineRowRow["EnabledIndicator"]),
                                CreatedBy = lineRowRow["CreatedBy"].ToString()
                            };

                            var createdDateTime = DataAccessHelper.ConvertToDateTime(lineRowRow["CreatedDateTime"]);
                            if (createdDateTime != null)
                                line.CreatedDateTime = createdDateTime.Value;

                            line.UpdatedBy = lineRowRow["UpdatedBy"].ToString();
                            createdDateTime = DataAccessHelper.ConvertToDateTime(lineRowRow["UpdatedDateTime"]);
                            if (createdDateTime != null)
                                line.UpdatedDateTime = createdDateTime.Value;

                            results.Add(line);
                        }
                    }

                    return results;
                }

       

                private static DataSet GetGridTemplatesForOrgFromDao(string orgId)
                {
                    var ds = OrdersDAO.Instance.GetGridTemplatesForOrg(orgId);
                    return ds;
                }
        
                private static string GetGridTemplatesCacheKey(string orgId)
                {
                    return string.Format(DistributedCacheKey.GridTemplatesCacheKey, orgId);
                }

                #endregion

        

       

                public static List<TimeZoneInfo> GetTimeZoneInfos()
                {
                    const string cacheKey = "___AvailableTimeZonesCacheKey";

                    var results =
                        VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as List<TimeZoneInfo>;

                    //if (results != null) return results;

                    var systemList = TimeZoneInfo.GetSystemTimeZones();

                    var listTopTimeZone = new List<string>
                                          {
                                              "Atlantic Standard Time",
                                              "Eastern Standard Time",
                                              "Central Standard Time",
                                              "Mountain Standard Time",
                                              "Pacific Standard Time",
                                              "Alaskan Standard Time",
                                              "Hawaiian Standard Time"
                                          };

                    var lst = new List<TimeZoneInfo>
                              {
                                  systemList.FirstOrDefault(i => i.Id == "Atlantic Standard Time"),
                                  systemList.FirstOrDefault(i => i.Id == "Eastern Standard Time"),
                                  systemList.FirstOrDefault(i => i.Id == "Central Standard Time"),
                                  systemList.FirstOrDefault(i => i.Id == "Mountain Standard Time"),
                                  systemList.FirstOrDefault(i => i.Id == "Pacific Standard Time"),
                                  systemList.FirstOrDefault(i => i.Id == "Alaskan Standard Time"),
                                  systemList.FirstOrDefault(i => i.Id == "Hawaiian Standard Time")
                              };
                    lst.AddRange(systemList.Where(i => !listTopTimeZone.Contains(i.Id)));

                    VelocityCacheManager.Write(cacheKey, lst, CommonCacheContant.Ts360FarmCacheName);
                    return lst;
                }

                private static void SetExpireCurrentUserReservedType()
                {
                    var userid = !string.IsNullOrEmpty(SiteContext.Current.UserId) ? SiteContext.Current.UserId.ToLower() : "";
                    var cachekey = "___CurrentInventoryType" + userid;
                    VelocityCacheManager.SetExpired(cachekey, VelocityCacheLevel.Session);
                }

       
                public static void UpdateCurrentUserReservedType()
                {
                    SetExpireCurrentUserReservedType();
                    GetCurrentUserReservedType();
                }

                private static string CollectCurrentUserReservedType()
                {
                    var sitecontext = SiteContext.Current;
                    if (sitecontext.MarketType == MarketType.AcademicLibrary ||
                        sitecontext.MarketType == MarketType.PublicLibrary ||
                        sitecontext.MarketType == MarketType.SchoolLibrary)
                    {
                        return "le";
                    }
                    //
                    var accountId = string.Empty;
                    if (string.IsNullOrEmpty(sitecontext.DefaultBookAccountId))
                    {
                        var org = AdministrationProfileController.Current.GetOrganization(sitecontext.OrganizationId);
                        if (org != null && org.DefaultBookAccount != null)
                        {
                            var orgDefaultAccount = org.DefaultBookAccount;
                            accountId = orgDefaultAccount.Id;
                        }
                        else
                        {
                            return "a";
                        }
                    }
                    else
                    {
                        accountId = sitecontext.DefaultBookAccountId;
                    }

                    var account = AdministrationProfileController.Current.GetAccountById(accountId);
                    if (account != null && account.AccountInventoryType.ToLower() == "d")
                        return "d";
                    else
                        return "a";
                }

        */

    }
}
