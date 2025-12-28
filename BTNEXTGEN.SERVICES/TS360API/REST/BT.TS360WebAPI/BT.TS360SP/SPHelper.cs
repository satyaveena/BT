using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BT.TS360API.Cache;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;

namespace BT.TS360SP
{
    public static class ContentManagementHelper
    {
        private static readonly List<string> ReferenceListName = new List<string>
                                                                     {
                                                                "eListCategory", 
                                                                "eListSubcategory",
                                                                "eList",
                                                                "Publication", 
                                                                "PublicationIssue", 
                                                                "PublicationCategory"
                                                            };

        public static Dictionary<string, string> AudienceType = new Dictionary<string, string>
                                                                    {
            {"Children_Grade_2_3_Age_7_8","ChildrenSGrade23Age78"},
            {"Teen_Grade_10_12_Age_15_18","TeenGrade1012Age1518"},            
            {"Children_Grade_3_4_Age_8_9","ChildrenSGrade34Age89"},
            {"Children_Kindergarten_Age_5_6","ChildrenSKindergartenAge56"},
            {"Children_Grade_1_2_Age_6_7","ChildrenSGrade12Age67"},
            {"Children_Grade_4_6_Age_9_11","ChildrenSGrade46Age911"},
            {"Teen_Grade_7-9_Age_12-14","TeenGrade79Age1214"},
            {"Scholarly_47_Graduate","ScholarlyGraduate"},
            {"Professional","Professional"},
            {"Children_Babies_Age_0_2","ChildrenSBabiesAge02"},
            {"Children_Toddlers_Age_2_4","ChildrenSToddlersAge24"},
            {"General_32_Adult","GeneralAdult"},
            {"Scholarly_47_Associate","ScholarlyAssociate"},
            {"Vocational_47_Technical","VocationalTechnical"},
            {"Scholarly_47_Undergraduate","ScholarlyUndergraduate"}
        };

        public static Dictionary<string, string> ProductType = new Dictionary<string, string>
                                                                   {
            {"0","Books"},
            {"1","Entertainment"},
            {"2","EntertainmentMusic"},
            {"3","EntertainmentMovies"}
        };

        public static Dictionary<string, string> MarketType = new Dictionary<string, string>
                                                                  {
            {"0","Retail"},
            {"1","Public Library"},
            {"2","Academic Library"},
            {"3","School Library"}
        };

        public static Dictionary<string, string> MarketTypeSiteTerm2CodeType = new Dictionary<string, string>
                                                                                   {
            {"Retail","Retail"},
            {"Public Library","PublicLibrary"},
            {"Academic Library","AcademicLibrary"},
            {"School Library","SchoolLibrary"}
        };

        public const string PRODUCT_TYPE = "ProductType";
        public const string AUDIENCE_TYPE = "AudienceType";
        public const string MARKET_TYPE = "MarketType";
        private const string ReplicatedReferenceField = "ReplicatedReferenceField";
        private const string ReplicatedReferenceFieldExpiredSite = "ExpiredItemReplicatedReferenceField";
        private const string BREADCRUMB_TITLE_FOR_RELEASE = "ALL {0}{1} NEW{2} RELEASES";
        private const string SPACE = " ";
        private const string MUSIC = " MUSIC";
        private const string MOVIE = " MOVIE";
        private const string BOOK = " BOOK";
        private const string DATE_FORMAT = "MMMM";
        private const string DAY_FORMAT = "dd";
        private const string TargetingContextAll = "ALL";
        private const string Delimiter = ";";

        //public static SPListItem CopySPListItem(string listName, SPListItem sourceItem, SPWeb targetWeb)
        //{
        //    targetWeb.AllowUnsafeUpdates = true;
        //    SPList targetList = targetWeb.Lists[listName];
        //    SPListItem newItem = targetList.Items.Add();
        //    foreach (SPField field in sourceItem.Fields)
        //    {
        //        if (field.StaticName == CMConstants.ApprovalStatusFieldName) continue;

        //        if (!field.ReadOnlyField &&
        //            field.Type != SPFieldType.Invalid &&
        //            field.Type != SPFieldType.Attachments &&
        //            field.Type != SPFieldType.File
        //            && field.Type != SPFieldType.Computed
        //            )
        //        {
        //            newItem[field.InternalName] = sourceItem[field.InternalName];
        //        }
        //    }

        //    newItem.Update();
        //    return newItem;
        //}
        //public static SPListItem CopySPListItemFromCollaborationToPublishing(string listName,
        //     SPListItem sourceItem,
        //     SPWeb targetWeb,
        //     string currentURl,
        //    string targetURL)
        //{
        //    targetWeb.AllowUnsafeUpdates = true;
        //    SPList targetList = targetWeb.Lists[listName];
        //    SPListItem newItem = targetList.Items.Add();
        //    if (ReferenceListName.Contains(listName.Trim()))
        //    {
        //        if (!targetList.Fields.ContainsField(ReplicatedReferenceField))
        //        {
        //            var newField = targetList.Fields.CreateNewField(SPFieldType.Number.ToString(),
        //                                                            ReplicatedReferenceField);
        //            newField.Hidden = true;
        //            targetList.Fields.Add(newField);
        //            targetList.Update();
        //        }
        //        newItem[ReplicatedReferenceField] = sourceItem.ID;
        //    }

        //    foreach (SPField field in sourceItem.Fields)
        //    {
        //        if (!field.ReadOnlyField)
        //        {
        //            if (field.Type == SPFieldType.Lookup)
        //            {
        //                var pubField = newItem.Fields.GetField(field.InternalName);
        //                if (pubField != null)
        //                {
        //                    var lookupField = (SPFieldLookup)pubField;
        //                    if (!String.IsNullOrEmpty(lookupField.LookupList))
        //                    {
        //                        var lookupListGuid = new Guid(lookupField.LookupList);
        //                        SPList parentList = targetWeb.Lists[lookupListGuid];
        //                        var parentValue = new SPFieldLookupValue(sourceItem[field.InternalName] as string);

        //                        SPQuery query = new SPQuery();
        //                        query.Query = CMConstants.WHERE_TAG_OPEN +
        //                                      CreateCamlGetItemByField(ReplicatedReferenceField,
        //                                                               parentValue.LookupId.ToString()) +
        //                                      CMConstants.WHERE_TAG_CLOSE;
        //                        SPListItemCollection items = parentList.GetItems(query);
        //                        if (items != null && items.Count > 0)
        //                        {
        //                            newItem[field.InternalName] = items[0].ID;
        //                        }
        //                    }
        //                }
        //            }
        //            else if (field.Type != SPFieldType.Invalid &&
        //                     field.Type != SPFieldType.Attachments &&
        //                     field.Type != SPFieldType.File
        //                     && field.Type != SPFieldType.Computed
        //                )
        //            {
        //                newItem[field.InternalName] = sourceItem[field.InternalName];
        //            }
        //            if (field.Type == SPFieldType.URL)
        //            {
        //                object oUrl = newItem[field.InternalName];
        //                if (oUrl == null) continue;
        //                string strUrl = oUrl.ToString();
        //                var url = new SPFieldUrlValue(strUrl);

        //                RefindUrlFieldToTargetSite(url, currentURl, targetURL);
        //                newItem[field.InternalName] = url;
        //            }
        //        }
        //    }
        //    newItem.Update();
        //    return newItem;
        //}

        //private static SPFieldLookup GetLookUpField(SPListItem item)
        //{
        //    foreach (SPField field in item.Fields)
        //    {
        //        if (!field.ReadOnlyField)
        //        {
        //            if (field.Type == SPFieldType.Lookup)
        //            {
        //                return (SPFieldLookup)item.Fields.GetField(field.InternalName);
        //            }
        //        }
        //    }
        //    return null;

        //}

        /// <summary>
        /// Check if its parent items is OK and if Item was copied to publishing
        /// </summary>
        /// <param name="parentSourceItem"></param>
        /// <param name="sourceItem"></param>
        /// <param name="targetWeb"></param>
        /// <param name="listName"></param>
        /// <returns></returns>
        //private static bool IsItemNotCopiedYet(SPListItem sourceItem, SPWeb targetWeb, string listName)
        //{
        //    SPList targetList = targetWeb.Lists[listName];
        //    SPQuery query = new SPQuery();
        //    query.Query = CMConstants.WHERE_TAG_OPEN +
        //                  CreateCamlGetItemByField(ReplicatedReferenceField, sourceItem.ID.ToString()) +
        //                  CMConstants.WHERE_TAG_CLOSE;
        //    SPListItemCollection items = targetList.GetItems(query);
        //    if (items != null && items.Count > 0)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private static bool CheckForParentItem(SPListItem item)
        //{
        //    var lookUpField = GetLookUpField(item);
        //    //
        //    //DateTime expirationDate;
        //    //DateTime.TryParse(item["ExpirationDate"].ToString(), out expirationDate);
        //    //
        //    var itemStatus = item["ItemStatus"] != null ? item["ItemStatus"].ToString() : string.Empty;
        //    //
        //    if (itemStatus == "Published")
        //    {
        //        if (lookUpField == null || string.IsNullOrEmpty(lookUpField.LookupList))
        //            return true;

        //        if (!String.IsNullOrEmpty(lookUpField.LookupList))
        //        {
        //            var lookupListGuid = new Guid(lookUpField.LookupList);
        //            var parentValue = new SPFieldLookupValue(item[lookUpField.InternalName] as string);
        //            var parentSourceList = item.Web.Lists[lookupListGuid];
        //            var parentSourceItem = parentSourceList.GetItemById(parentValue.LookupId);
        //            //
        //            return CheckForParentItem(parentSourceItem);
        //        }
        //        return false;
        //    }
        //    return false;
        //}

        ////This is just update to Publishing site... item should already be there
        //public static bool UpdateItemFromCollaborationToPublishing(string listName, SPListItem sourceItem, SPWeb targetWeb, string collaborationSiteUrl, string publishingSiteUrl)
        //{
        //    //
        //    SPList targetList = targetWeb.Lists[listName];
        //    if (targetList.Fields.ContainsField(ReplicatedReferenceField) && !IsItemNotCopiedYet(sourceItem, targetWeb, listName))
        //    {
        //        SPQuery query = new SPQuery();
        //        query.Query = CMConstants.WHERE_TAG_OPEN +
        //                      CreateCamlGetItemByField(ReplicatedReferenceField, sourceItem.ID.ToString()) +
        //                      CMConstants.WHERE_TAG_CLOSE;
        //        SPListItemCollection items = targetList.GetItems(query);
        //        if (items != null && items.Count > 0)
        //        {
        //            //Update item status
        //            var item = items[0];
        //            targetWeb.AllowUnsafeUpdates = true;
        //            item["ItemStatus"] = "Expired"; ;
        //            item.Update();
        //            targetWeb.AllowUnsafeUpdates = false;
        //            return true;
        //        }
        //    }
        //    return false;
        //    //
        //}

        //public static bool CopyListItemFromCollaborationToPublishingNWF(string listName,
        //            SPListItem sourceItem,
        //            SPWeb targetWeb,
        //            string currentURl,
        //            string targetURL)
        //{

        //    SPList targetList = targetWeb.Lists[listName];
        //    //
        //    if (targetList.Fields.ContainsField(ReplicatedReferenceField) && !IsItemNotCopiedYet(sourceItem, targetWeb, listName))
        //    {
        //        return false;
        //    }
        //    //
        //    foreach (SPField field in sourceItem.Fields)
        //    {
        //        if (!field.ReadOnlyField)
        //        {
        //            if (field.Type == SPFieldType.Lookup)
        //            {
        //                //
        //                var collabField = sourceItem.Fields.GetField(field.InternalName);
        //                //
        //                if (collabField != null)
        //                {
        //                    var lookupField = (SPFieldLookup)collabField;

        //                    if (!String.IsNullOrEmpty(lookupField.LookupList))
        //                    {
        //                        var lookupListGuid = new Guid(lookupField.LookupList);
        //                        var parentValue = new SPFieldLookupValue(sourceItem[field.InternalName] as string);
        //                        var parentSourceList = sourceItem.Web.Lists[lookupListGuid];
        //                        var parentSourceItem = parentSourceList.GetItemById(parentValue.LookupId);
        //                        if (!CheckForParentItem(parentSourceItem))
        //                            return false;

        //                    }
        //                }

        //            }
        //        }
        //    }

        //    targetWeb.AllowUnsafeUpdates = true;
        //    SPListItem newItem = targetList.Items.Add();
        //    //if (ReferenceListName.Contains(listName.Trim()))
        //    //{
        //    if (!targetList.Fields.ContainsField(ReplicatedReferenceField))
        //    {
        //        var newField = targetList.Fields.CreateNewField(SPFieldType.Number.ToString(),
        //                                                        ReplicatedReferenceField);
        //        newField.Hidden = true;
        //        targetList.Fields.Add(newField);
        //        targetList.Update();
        //    }
        //    newItem[ReplicatedReferenceField] = sourceItem.ID;
        //    //}

        //    foreach (SPField field in sourceItem.Fields)
        //    {
        //        if (!field.ReadOnlyField)
        //        {
        //            if (field.Type == SPFieldType.Lookup)
        //            {
        //                var pubField = newItem.Fields.GetField(field.InternalName);
        //                if (pubField != null)
        //                {
        //                    var lookupField = (SPFieldLookup)pubField;
        //                    if (!String.IsNullOrEmpty(lookupField.LookupList))
        //                    {
        //                        var lookupListGuid = new Guid(lookupField.LookupList);
        //                        var parentValue = new SPFieldLookupValue(sourceItem[field.InternalName] as string);
        //                        SPList parentList = targetWeb.Lists[lookupListGuid];
        //                        SPQuery query = new SPQuery();
        //                        query.Query = CMConstants.WHERE_TAG_OPEN +
        //                                      CreateCamlGetItemByField(ReplicatedReferenceField,
        //                                                               parentValue.LookupId.ToString()) +
        //                                      CMConstants.WHERE_TAG_CLOSE;
        //                        SPListItemCollection items = parentList.GetItems(query);

        //                        if (items != null && items.Count > 0)
        //                        {
        //                            newItem[field.InternalName] = items[0].ID;
        //                        }
        //                    }
        //                }
        //            }

        //            else if (field.Type != SPFieldType.Invalid &&
        //                     field.Type != SPFieldType.Attachments &&
        //                     field.Type != SPFieldType.File
        //                     && field.Type != SPFieldType.Computed
        //                )
        //            {
        //                newItem[field.InternalName] = sourceItem[field.InternalName];
        //            }

        //            if (field.Type == SPFieldType.URL)
        //            {
        //                object oUrl = newItem[field.InternalName];
        //                if (oUrl == null) continue;
        //                string strUrl = oUrl.ToString();
        //                var url = new SPFieldUrlValue(strUrl);

        //                RefindUrlFieldToTargetSite(url, currentURl, targetURL);
        //                newItem[field.InternalName] = url;
        //            }

        //        }
        //    }
        //    //New Requirement: Change item in publishing to Published
        //    newItem["ItemStatus"] = "Published";
        //    //
        //    newItem.Update();
        //    return true;
        //}

        /// <summary>
        /// Copy expired items from Collaboration to Expired
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="sourceItem"></param>
        /// <param name="targetWeb"></param>
        /// <param name="currentURl"></param>
        /// <param name="targetURL"></param>
        /// <returns></returns>
        //public static bool CopySPListItemFromCollaborationToExpired(string listName,
        //            SPListItem sourceItem,
        //            SPWeb targetWeb,
        //            string currentURl,
        //            string targetURL)
        //{

        //    SPList targetList = targetWeb.Lists[listName];
        //    //
        //    if (targetList.Fields.ContainsField(ReplicatedReferenceFieldExpiredSite) && !IsItemNotCopiedYet(sourceItem, targetWeb, listName))
        //    {
        //        return false;
        //    }
        //    //
        //    foreach (SPField field in sourceItem.Fields)
        //    {
        //        if (!field.ReadOnlyField)
        //        {
        //            if (field.Type == SPFieldType.Lookup)
        //            {
        //                //
        //                var collabField = sourceItem.Fields.GetField(field.InternalName);
        //                //
        //                if (collabField != null)
        //                {
        //                    var lookupField = (SPFieldLookup)collabField;

        //                    if (!String.IsNullOrEmpty(lookupField.LookupList))
        //                    {
        //                        var lookupListGuid = new Guid(lookupField.LookupList);
        //                        var parentValue = new SPFieldLookupValue(sourceItem[field.InternalName] as string);
        //                        var parentSourceList = sourceItem.Web.Lists[lookupListGuid];
        //                        var parentSourceItem = parentSourceList.GetItemById(parentValue.LookupId);
        //                        if (!CheckForParentItem(parentSourceItem))
        //                            return false;

        //                    }
        //                }

        //            }
        //        }
        //    }

        //    targetWeb.AllowUnsafeUpdates = true;
        //    SPListItem newItem = targetList.Items.Add();
        //    //if (ReferenceListName.Contains(listName.Trim()))
        //    //{
        //    if (!targetList.Fields.ContainsField(ReplicatedReferenceFieldExpiredSite))
        //    {
        //        var newField = targetList.Fields.CreateNewField(SPFieldType.Number.ToString(),
        //                                                        ReplicatedReferenceFieldExpiredSite);
        //        newField.Hidden = true;
        //        targetList.Fields.Add(newField);
        //        targetList.Update();
        //    }
        //    newItem[ReplicatedReferenceFieldExpiredSite] = sourceItem.ID;
        //    //}

        //    foreach (SPField field in sourceItem.Fields)
        //    {
        //        if (!field.ReadOnlyField)
        //        {
        //            if (field.Type == SPFieldType.Lookup)
        //            {
        //                var pubField = newItem.Fields.GetField(field.InternalName);
        //                if (pubField != null)
        //                {
        //                    var lookupField = (SPFieldLookup)pubField;
        //                    if (!String.IsNullOrEmpty(lookupField.LookupList))
        //                    {
        //                        var lookupListGuid = new Guid(lookupField.LookupList);
        //                        var parentValue = new SPFieldLookupValue(sourceItem[field.InternalName] as string);
        //                        SPList parentList = targetWeb.Lists[lookupListGuid];
        //                        SPQuery query = new SPQuery();
        //                        query.Query = CMConstants.WHERE_TAG_OPEN +
        //                                        CreateCamlGetItemByField(ReplicatedReferenceFieldExpiredSite,
        //                                                                parentValue.LookupId.ToString()) +
        //                                        CMConstants.WHERE_TAG_CLOSE;
        //                        SPListItemCollection items = parentList.GetItems(query);

        //                        if (items != null && items.Count > 0)
        //                        {
        //                            newItem[field.InternalName] = items[0].ID;
        //                        }
        //                    }
        //                }
        //            }

        //            else if (field.Type != SPFieldType.Invalid &&
        //                        field.Type != SPFieldType.Attachments &&
        //                        field.Type != SPFieldType.File
        //                        && field.Type != SPFieldType.Computed
        //                )
        //            {
        //                newItem[field.InternalName] = sourceItem[field.InternalName];
        //            }

        //            if (field.Type == SPFieldType.URL)
        //            {
        //                object oUrl = newItem[field.InternalName];
        //                if (oUrl == null) continue;
        //                string strUrl = oUrl.ToString();
        //                var url = new SPFieldUrlValue(strUrl);

        //                RefindUrlFieldToTargetSite(url, currentURl, targetURL);
        //                newItem[field.InternalName] = url;
        //            }
        //        }
        //    }
        //    //
        //    newItem.Update();
        //    return true;
        //}

        //private static void RefindUrlFieldToTargetSite(SPFieldUrlValue url, string currentURl, string targetUrl)
        //{
        //    string strUrl = url.Url;
        //    if (!String.IsNullOrEmpty(strUrl))
        //        url.Url = strUrl.Replace(currentURl, targetUrl);
        //    string desc = url.Description;
        //    if (!String.IsNullOrEmpty(desc))
        //        url.Description = desc.Replace(currentURl, targetUrl);
        //}

        /// <summary>
        /// if request protocal is "https", change current url to "https". It happens same as "http"
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RefineUrlProtocolToContextProtocol(string url, string scheme)
        {
            if (!String.IsNullOrEmpty(url))
            {
                string contextScheme = scheme.ToLower();
                string tempUrl = url.ToLower();
                if (String.Compare(contextScheme, CMConstants.HTTPS) == 0)
                {
                    if (tempUrl.StartsWith(CMConstants.HTTPS))
                    {
                        return String.Format("{0}{1}", CMConstants.HTTPS, url.Substring(5));
                    }
                    else if (tempUrl.StartsWith(CMConstants.HTTP))
                    {
                        return String.Format("{0}{1}", CMConstants.HTTPS, url.Substring(4));
                    }
                }
                else if (String.Compare(contextScheme, CMConstants.HTTP) == 0)
                {
                    if (tempUrl.StartsWith(CMConstants.HTTPS))
                    {
                        return String.Format("{0}{1}", CMConstants.HTTP, url.Substring(5));
                    }
                    else if (tempUrl.StartsWith(CMConstants.HTTP))
                    {
                        return String.Format("{0}{1}", CMConstants.HTTP, url.Substring(4));
                    }
                }
            }
            return url;
        }

        public static string GetRelativePath(string url, string domainName)
        {
            string relativeUrl = url;
            if (!string.IsNullOrWhiteSpace(url) && url.ToLower().Contains(domainName.ToLower()))
            {
                var uri = new Uri(url);
                relativeUrl = uri.PathAndQuery;
            }
            
            return relativeUrl;
        }

        public static string RefinePromoTargetPage(string promotionCode)
        {
            if (String.IsNullOrEmpty(promotionCode))
                return SiteUrl.PromotionDetails;

            return SiteUrl.PromotionProducts;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">input.ItemDataText is the AdName</param>
        /// <returns></returns>
        public static string BuildCamlString(Collection<CampaignAdItem> input)
        {
            const string adNameFieldName = "AdName";

            var firstPart = new StringBuilder();
            var secondPart = new StringBuilder();

            if (input.Count == 1)
            {
                firstPart.AppendFormat("<Eq><FieldRef Name=\"{0}\" /><Value Type=\"Text\">{1}</Value></Eq>", adNameFieldName, input[0].AdName);
            }
            else if (input.Count > 1)
            {
                firstPart.Append(CMConstants.OR_TAG_OPEN);
                firstPart.AppendFormat("<Eq><FieldRef Name=\"{0}\" /><Value Type=\"Text\">{1}</Value></Eq>", adNameFieldName,
                            input[0].AdName);
                var nextToLastIndex = input.Count - 1;
                for (var i = 1; i < input.Count; i++)
                {
                    if (i != nextToLastIndex)
                    {
                        firstPart.Append(CMConstants.OR_TAG_OPEN);
                    }
                    firstPart.AppendFormat("<Eq><FieldRef Name=\"{0}\" /><Value Type=\"Text\">{1}</Value></Eq>", adNameFieldName,
                        input[i].AdName);
                    secondPart.Append(CMConstants.OR_TAG_CLOSE);
                }
            }

            return String.Format("{0}{1}", firstPart, secondPart);
        }

        public static string BuildCamlString(List<string> list, string fieldName)
        {
            var firstPart = new StringBuilder();
            var secondPart = new StringBuilder();

            if (list.Count == 1)
            {
                firstPart.AppendFormat("<Eq><FieldRef Name=\"{0}\" /><Value Type=\"Choice\">{1}</Value></Eq>", fieldName, list[0]);
            }
            else if (list.Count > 1)
            {
                firstPart.Append(CMConstants.OR_TAG_OPEN);
                firstPart.AppendFormat("<Eq><FieldRef Name=\"{0}\" /><Value Type=\"Choice\">{1}</Value></Eq>", fieldName, list[0]);
                var nextToLastIndex = list.Count - 1;
                for (var i = 1; i < list.Count; i++)
                {
                    if (i != nextToLastIndex)
                    {
                        firstPart.Append(CMConstants.OR_TAG_OPEN);
                    }
                    firstPart.AppendFormat("<Eq><FieldRef Name=\"{0}\" /><Value Type=\"Choice\">{1}</Value></Eq>", fieldName, list[i]);
                    secondPart.Append(CMConstants.OR_TAG_CLOSE);
                }
            }

            return String.Format("{0}{1}", firstPart, secondPart);
        }

        public static string CreateCamlGetByParentId(string internalFieldName, string parentId)
        {
            return String.Format("{0}<FieldRef Name='{1}' LookupId='true'/><Value Type='Lookup'>{2}</Value>{3}",
                                CMConstants.EQ_TAG_OPEN, internalFieldName, parentId, CMConstants.EQ_TAG_CLOSE);
        }
        public static string CreateCamlGetByParentValue(string internalFieldName, string parentValue)
        {
            return String.Format("{0}<FieldRef Name='{1}'/><Value Type='Lookup'>{2}</Value>{3}",
                                CMConstants.EQ_TAG_OPEN, internalFieldName, parentValue, CMConstants.EQ_TAG_CLOSE);
        }

        public static string CreateCamlGetById(string id)
        {
            return String.Format("{0}<FieldRef Name='{1}'/><Value Type='Number'>{2}</Value>{3}",
                                CMConstants.EQ_TAG_OPEN, "ID", id, CMConstants.EQ_TAG_CLOSE);
        }

        public static string CreateCamlGetItemByField(string fieldName, string value)
        {
            return String.Format("{0}<FieldRef Name='{1}'/><Value Type='Number'>{2}</Value>{3}",
                                CMConstants.EQ_TAG_OPEN, fieldName, value, CMConstants.EQ_TAG_CLOSE);
        }

        public static string CreateCamlEqualBoolValue(string fieldName, bool value)
        {
            return String.Format("{0}<FieldRef Name='{1}'/><Value Type='Integer'>{2}</Value>{3}",
                CMConstants.EQ_TAG_OPEN, fieldName, value ? 1 : 0, CMConstants.EQ_TAG_CLOSE);
        }

        public static string CreateCamlWhereExpression(string expression)
        {
            return String.Format("{0}{1}{2}", CMConstants.WHERE_TAG_OPEN, expression, CMConstants.WHERE_TAG_CLOSE);
        }

        public static string CreateCamlOrderExpression(string expression)
        {
            return String.Format("{0}{1}{2}", CMConstants.ORDERBY_TAG_OPEN, expression, CMConstants.ORDERBY_TAG_CLOSE);
        }

        public static void RefineListItems<T>(IList<T> items, int numberOfTopItemsSelected) where T : class
        {
            if (items == null)
                return;
            var itemCount = items.Count;

            if (itemCount <= numberOfTopItemsSelected) return;

            for (var i = 0; i < itemCount; i++)
            {
                if (i >= numberOfTopItemsSelected) items.RemoveAt(numberOfTopItemsSelected);
            }
        }

        //public static string GetSearchedBtKeyByProductType(ProductSearchResults results, ProductType productType)
        //{
        //    if (results == null || results.Items == null || results.Items.Count == 0)
        //        return String.Empty;
        //    var items = results.Items;

        //    var btKeyList = new List<string>();

        //    btKeyList.AddRange(from prod in items
        //                       where productType == ContentManagement.ProductType.All ||
        //                       productType.ToString().ToLower().Contains(prod.ProductType.ToLower())
        //                       select prod.BTKey);


        //    return String.Join("|", btKeyList.ToArray());
        //}

        

        public static string CombineBtKeysForSearch(IEnumerable<SingleProductBaseItem> items)
        {
            if (items == null) return string.Empty;

            var result = items.Aggregate(String.Empty, (current, item) => current + (item.BTKEY + SearchQueryStringName.KEYWORD_BTKEYSEPARATOR));
            result = result.TrimEnd(SearchQueryStringName.KEYWORD_BTKEYSEPARATOR);
            return result;
        }

        //public static ProductType ProductTypeFromConvertString(string strValue)
        //{
        //    var result = ContentManagement.ProductType.None;
        //    if (String.IsNullOrEmpty(strValue))
        //    {
        //        return result;
        //    }
        //    switch (strValue)
        //    {
        //        case "All":
        //            result = ContentManagement.ProductType.All;
        //            break;
        //        case "Book":
        //        case "Books":
        //            result = ContentManagement.ProductType.Books;
        //            break;
        //        case "Music":
        //            result = ContentManagement.ProductType.EntertainmentMusic;
        //            break;
        //        case "Movie":
        //        case "Movies":
        //            result = ContentManagement.ProductType.EntertainmentMovies;
        //            break;
        //        default:
        //            result = ContentManagement.ProductType.Invalid;
        //            break;
        //    }
        //    return result;
        //}

        public static string RefineUrlFromAuthToInternet(string itemUrl)
        {
            if (itemUrl == null) throw new ArgumentNullException("itemUrl");

            var internetSiteUrl = AppSettings.InternetURL;
            var authenticatedSiteUrl = AppSettings.AuthURL;
           
            if (itemUrl.IndexOf(authenticatedSiteUrl,StringComparison.OrdinalIgnoreCase) == 0)
            {
                itemUrl = itemUrl.Replace(authenticatedSiteUrl, internetSiteUrl);
            }
            return itemUrl;
        }

        //public static string GenerateTitleForBrowseResultsCalendar()
        //{
        //    var releaseDay = HttpContext.Current.Request.QueryString[SearchFieldNameConstants.releaseday];
        //    var releaseMonth = HttpContext.Current.Request.QueryString[SearchFieldNameConstants.releasemonth];
        //    var releaseYear = HttpContext.Current.Request.QueryString[SearchFieldNameConstants.releaseyear];
        //    var releaseProductTypeQueryString = HttpContext.Current.Request.QueryString[SearchFieldNameConstants.releaseproducttype] ?? string.Empty;
        //    var releaseProductType = ContentManagementHelper.ProductTypeFromConvertString(releaseProductTypeQueryString);

        //    var date = new DateTime();
        //    DateTime.TryParse(string.Format("{0}/{1}/{2}", releaseMonth ?? "1", releaseDay ?? "1", releaseYear ?? "1"), out date);
        //    //View all releases of a month

        //    if (string.IsNullOrEmpty(releaseDay) &&
        //        releaseProductType == ContentManagement.ProductType.All)
        //    {
        //        return string.Format(BREADCRUMB_TITLE_FOR_RELEASE,
        //            date.ToString(DATE_FORMAT),
        //            string.Empty,
        //            string.Empty);
        //    }
        //    //View book releases of a month
        //    if (string.IsNullOrEmpty(releaseDay) &&
        //        releaseProductType == ContentManagement.ProductType.Books)
        //    {
        //        return string.Format(BREADCRUMB_TITLE_FOR_RELEASE,
        //            date.ToString(DATE_FORMAT),
        //            string.Empty,
        //            BOOK);
        //    }
        //    //View movie releases of a month
        //    if (string.IsNullOrEmpty(releaseDay) &&
        //        releaseProductType == ContentManagement.ProductType.EntertainmentMovies)
        //    {
        //        return string.Format(BREADCRUMB_TITLE_FOR_RELEASE,
        //            date.ToString(DATE_FORMAT),
        //            string.Empty,
        //            MOVIE);
        //    }
        //    //View music releases of a month
        //    if (string.IsNullOrEmpty(releaseDay) &&
        //        releaseProductType == ContentManagement.ProductType.EntertainmentMusic)
        //    {
        //        return string.Format(BREADCRUMB_TITLE_FOR_RELEASE,
        //            date.ToString(DATE_FORMAT),
        //            string.Empty,
        //            MUSIC);
        //    }
        //    //View all releases of a day

        //    if (!string.IsNullOrEmpty(releaseDay) &&
        //        releaseProductType == ContentManagement.ProductType.All)
        //    {
        //        return string.Format(BREADCRUMB_TITLE_FOR_RELEASE,
        //            date.ToString(DATE_FORMAT),
        //            SPACE + date.ToString(DAY_FORMAT) + GetDaySuffix(date.Day),
        //            string.Empty);
        //    }
        //    //View book releases of a day
        //    if (!string.IsNullOrEmpty(releaseDay) &&
        //        releaseProductType == ContentManagement.ProductType.Books)
        //    {
        //        return string.Format(BREADCRUMB_TITLE_FOR_RELEASE,
        //            date.ToString(DATE_FORMAT),
        //            SPACE + date.ToString(DAY_FORMAT) + GetDaySuffix(date.Day),
        //            BOOK);
        //    }
        //    //View music releases of a day
        //    if (!string.IsNullOrEmpty(releaseDay) &&
        //        releaseProductType == ContentManagement.ProductType.EntertainmentMusic)
        //    {
        //        return string.Format(BREADCRUMB_TITLE_FOR_RELEASE,
        //            date.ToString(DATE_FORMAT),
        //            SPACE + date.ToString(DAY_FORMAT) + GetDaySuffix(date.Day),
        //            MUSIC);
        //    }
        //    //View movie releases of a day
        //    if (!string.IsNullOrEmpty(releaseDay) &&
        //        releaseProductType == ContentManagement.ProductType.EntertainmentMovies)
        //    {
        //        return string.Format(BREADCRUMB_TITLE_FOR_RELEASE,
        //            date.ToString(DATE_FORMAT),
        //            SPACE + date.ToString(DAY_FORMAT) + GetDaySuffix(date.Day),
        //            MOVIE);
        //    }
        //    return string.Empty;
        //}

        public static string GetDaySuffix(int day)
        {
            if (day == 1 || day == 21 || day == 31)
            {
                return "st";
            }
            else if (day == 2 || day == 22)
            {
                return "nd";
            }
            else if (day == 3 || day == 23)
            {
                return "rd";
            }
            else
            {
                return "th";
            }
        }
        //public static string ConvertPromotionFolderToString(string value)
        //{
        //    if (value == PromotionFolder.Promotion1.ToString())
        //        return "Promotion 1";
        //    else if (value == PromotionFolder.Promotion2.ToString())
        //        return "Promotion 2";
        //    else if (value == PromotionFolder.Promotion3.ToString())
        //        return "Promotion 3";
        //    else if (value == PromotionFolder.Promotion4.ToString())
        //        return "Promotion 4";
        //    else if (value == PromotionFolder.Promotion5.ToString())
        //        return "Promotion 5";
        //    else if (value == PromotionFolder.Promotion6.ToString())
        //        return "Promotion 6";
        //    else if (value == PromotionFolder.Promotion7.ToString())
        //        return "Promotion 7";
        //    else if (value == PromotionFolder.Promotion8.ToString())
        //        return "Promotion 8";
        //    else if (value == PromotionFolder.Promotion9.ToString())
        //        return "Promotion 9";
        //    else if (value == PromotionFolder.Promotion10.ToString())
        //        return "Promotion 10";
        //    else
        //        return string.Empty;
        //}

        public static TargetingParam ConvertStringToTargetingParam(string value)
        {
            TargetingParam targeting = new TargetingParam();
            var values = value.Split(new string[] { "|" }, StringSplitOptions.None);
            targeting.PIG = values[0];
            targeting.SiteBranding = values[1];
            targeting.MarketType = values[2];
            targeting.ProductType = values[3];
            targeting.AudienceType = values[4];
            targeting.OrgId = values[5];
            targeting.OrgName = values[6];
            return targeting;
        }

        public static TargetingParam ConvertStringToTargetingParamNoOrgInfo(string value)
        {
            var targeting = new TargetingParam();
            var values = value.Split(new string[] { "|" }, StringSplitOptions.None);
            targeting.PIG = values[0];
            targeting.SiteBranding = values[1];
            targeting.MarketType = values[2];
            targeting.ProductType = values[3];
            targeting.AudienceType = values[4];
            return targeting;
        }

        //public static List<ItemData> GetILSDocuments()
        //{
        //    var docs = CacheHelper.Get("__ILSReference__") as List<ItemData>;
        //    if (docs == null)
        //    {
        //        var publishedSiteUrl = CSUploadSharepointHelper.GetPublishingWebUrl();
        //        if (string.IsNullOrEmpty(publishedSiteUrl))
        //            return null;
        //        SPSecurity.RunWithElevatedPrivileges(delegate
        //        {
        //            using (SPSite site = new SPSite(publishedSiteUrl))
        //            {
        //                using (SPWeb web = site.RootWeb)
        //                {
        //                    var spList = web.Lists["ILSReference"];
        //                    List<ItemData> data = new List<ItemData>();
        //                    foreach (SPListItem item in spList.Items)
        //                    {
        //                        var caption = item["Caption"] != null ? item["Caption"].ToString() : string.Empty;
        //                        var subcaption = item["SubCaption"] != null ? item["SubCaption"].ToString() : string.Empty;
        //                        data.Add(new ItemData(caption, "/sites/publishing" + '/' + item.Url, subcaption));
        //                    }
        //                    docs = data.OrderBy(item => item.ItemDataValue).ToList();
        //                    CacheHelper.Write("__ILSReference__", docs);
        //                }
        //            }
        //        });
        //    }
        //    return docs;
        //}
    }
}
