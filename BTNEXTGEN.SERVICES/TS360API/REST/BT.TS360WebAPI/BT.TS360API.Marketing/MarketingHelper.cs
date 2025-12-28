using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using BT.TS360API.Cache;
using BT.TS360API.ServiceContracts;

namespace BT.TS360API.Marketing
{
    public class MarketingHelper
    {
        private const string TermName = "term";
        private const string NodeType = "TYPE";
        private const string NodeTypeAndValue = "and";
        private const string OperAttr = "OPER";
        private const string OperAttrEqualValue = "equal";
        private const string OperAttrNotEqualValue = "not-equal";
        private const string OperAttrContainsValue = "contains";
        private const string OperAttrNotContainsValue = "not-contains";
        private const string OperAttrIsTrueValue = "is-true";
        private const string OperAttrIsFalseValue = "is-false";
        private const string IdAttr = "ID";
        private const string TargetAudienceFieldName = "TargetingContext.audience_type";
        private const string TargetMarketTypeFieldName = "TargetingContext.market_type";
        private const string TargetProductTypeFieldName = "TargetingContext.product_type";
        private const string TargetSiteBrandingFieldName = "TargetingContext.SiteBranding";
        private const string TargetPigFieldName = "TargetingContext.ProductInterestGroup";
        private const string TargetOrgIdFieldName = "TargetingContext.org_id";
        private const string TargetOrgNameFieldName = "TargetingContext.org_name";

        private const string Delimiter = ";";

        public const string TargetingContextAll = "ALL";

        private static volatile MarketingHelper _instance;
        private static readonly object SyncRoot = new Object();

        private MarketingHelper() { }

        public static MarketingHelper Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MarketingHelper();
                }

                return _instance;
            }
        }

        public TargetingParam ConvertStringToTargetingParamNoOrgInfo(string value)
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

        public string GenerateTargetingValues(TargetingValues siteContext)
        {
            var delimeter = "|";
            var pigName = siteContext.PIGName;
            if (string.IsNullOrEmpty(pigName))
            {
                pigName = MarketingController.GeneratePigNameAllValue(pigName);
            }
            var siteBranding = siteContext.SiteBranding;
            var marketType = siteContext.MarketType == null ? string.Empty : ((int)siteContext.MarketType).ToString();
            var productType = MarketingController.ConvertToMultipleValue(siteContext.ProductType);
            var audienceType = MarketingController.ConvertToMultipleValue(siteContext.AudienceType);
            var orgId = TargetingContextAll + Delimiter + siteContext.OrgId;
            var orgName = TargetingContextAll + Delimiter + siteContext.OrganizationName;

            var sb = new StringBuilder();
            sb.Append(pigName);
            sb.Append(delimeter);
            sb.Append(siteBranding);
            sb.Append(delimeter);
            sb.Append(marketType);
            sb.Append(delimeter);
            sb.Append(productType);
            sb.Append(delimeter);
            sb.Append(audienceType);
            sb.Append(delimeter);
            sb.Append(orgId);
            sb.Append(delimeter);
            sb.Append(orgName);
            return sb.ToString();
        }
        public TargetingParam ToTargetingParam(TargetingValues siteContext)
        {
            var targeting = new TargetingParam();
            var pigName = siteContext.PIGName;
            if (string.IsNullOrEmpty(pigName))
            {
                pigName = MarketingController.GeneratePigNameAllValue(pigName);
            }

            targeting.PIG = pigName;
            targeting.SiteBranding = siteContext.SiteBranding;
            targeting.MarketType = siteContext.MarketType == null ? string.Empty : ((int)siteContext.MarketType).ToString();
            targeting.ProductType = MarketingController.ConvertToMultipleValue(siteContext.ProductType);
            targeting.AudienceType = MarketingController.ConvertToMultipleValue(siteContext.AudienceType);
            targeting.OrgId = TargetingContextAll + Delimiter + siteContext.OrgId;
            targeting.OrgName = TargetingContextAll + Delimiter + siteContext.OrganizationName;
            return targeting;
        }
        public TargetingParam ConvertStringToTargetingParam(string value)
        {
            var targeting = new TargetingParam();
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

        public string EncryptMd5(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash, input);
            }
        }
        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        public string GenerateTargetingQueryString(TargetingValues current)
        {
            var str = GenerateTargetingValues(current);
            var cacheKey = EncryptMd5(str);
            var cache = CachingController.Instance.Read(cacheKey + "_Text"); // VelocityCacheManager.Read(cacheKey + "_Text") as string;
            if (cache == null)
            {
                CachingController.Instance.Write(cacheKey + "_Text", str);
            }
            return cacheKey;
        }
        public string GenerateTargetingQueryStringNoOrgInfo(TargetingValues current)
        {
            var str = GenerateTargetingValuesNoOrgInfo(current);
            var cacheKey = EncryptMd5(str);
            return cacheKey;
        }
        public string GetTargetingText(TargetingValues current, ref string cachekey)
        {
            var targetingText = CachingController.Instance.Read(cachekey + "_Text") as string;
            if (targetingText != null) return targetingText;

            targetingText = GenerateTargetingValues(current);
            cachekey = EncryptMd5(targetingText);
            CachingController.Instance.Write(cachekey + "_Text", targetingText);
            return targetingText;
        }

        public string GenerateTargetingValuesNoOrgInfo(TargetingValues current)
        {
            if (current == null)
            {
                return "";
            }
            else
            {
            const string delimeter = "|";
            var pigName = current.PIGName;
            if (string.IsNullOrEmpty(pigName))
            {
                pigName = MarketingController.GeneratePigNameAllValue(pigName);
            }
            var siteBranding = current.SiteBranding;
            var marketType = current.MarketType == null ? string.Empty : ((int)current.MarketType).ToString();
            var productType = MarketingController.ConvertToMultipleValue(current.ProductType);
            var audienceType = MarketingController.ConvertToMultipleValue(current.AudienceType);
            var sb = new StringBuilder();
            sb.Append(pigName);
            sb.Append(delimeter);
            sb.Append(siteBranding);
            sb.Append(delimeter);
            sb.Append(marketType);
            sb.Append(delimeter);
            sb.Append(productType);
            sb.Append(delimeter);
            sb.Append(audienceType);
            return sb.ToString();
        }
        }
        public TargetingParam ToTargetingParamNoOrg(TargetingValues siteContext)
        {
            var targeting = new TargetingParam();
            var pigName = siteContext.PIGName;
            if (string.IsNullOrEmpty(pigName))
            {
                pigName = MarketingController.GeneratePigNameAllValue(pigName);
            }

            targeting.PIG = pigName;
            targeting.SiteBranding = siteContext.SiteBranding;
            targeting.MarketType = siteContext.MarketType == null ? string.Empty : ((int)siteContext.MarketType).ToString();
            targeting.ProductType = MarketingController.ConvertToMultipleValue(siteContext.ProductType);
            targeting.AudienceType = MarketingController.ConvertToMultipleValue(siteContext.AudienceType);
            return targeting;
        }
        public MarketingApprovedDiscount GetAppropriateDiscount(List<MarketingApprovedDiscount> approvedDiscounts, TargetingParam targetingParam,
            BtKeyParentCategoryObject parentCategory)
        {
            var vcParentCategory = string.Format("{0}({1})", parentCategory.ParentCategory, "MarketingCatalog");
            var vcParentProduct = string.Format("{0}({1})", parentCategory.BTKey, parentCategory.CatalogName);

            foreach (var approvedDis in approvedDiscounts)
            {
                if (parentCategory.BTKey == approvedDis.AwardProduct
                    || vcParentProduct == approvedDis.AwardProduct
                    || vcParentCategory == approvedDis.AwardCategory
                    || parentCategory.ParentCategory == approvedDis.AwardCategory)
                {
                    if (approvedDis.TargetingExpressions != null && approvedDis.TargetingExpressions.Count > 0)
                    {
                        var toBeContinued = false;
                        foreach (var targetExpr in approvedDis.TargetingExpressions)
                        {
                            var isTarget = TargetEligibility(targetExpr.TargetingBody, targetingParam);

                            if (!isTarget)
                            {
                                toBeContinued = true;
                                break;
                            }

                            if (targetExpr.TargetingAction == TargetingAction.Exclude)
                            {
                                toBeContinued = true;
                                break;
                            }
                        }

                        if (toBeContinued) continue;

                        return approvedDis;
                    }

                    if (approvedDis.EligibilityExpressions != null && approvedDis.EligibilityExpressions.Count > 0)
                    {
                        var toBeContinued = false;
                        foreach (var expr in approvedDis.EligibilityExpressions)
                        {
                            var isTarget = TargetEligibility(expr, targetingParam);

                            if (!isTarget)
                            {
                                toBeContinued = true;
                                break;
                            }
                        }

                        if (toBeContinued) continue;

                        return approvedDis;
                    }
                }
            }
            return null;
        }

        public List<MarketingApprovedAd> GetAppropriateAds(List<MarketingApprovedAd> approvedAds, TargetingParam targetingParam)
        {
            var results = new List<MarketingApprovedAd>();

            foreach (var approvedAd in approvedAds)
            {
                if (approvedAd.TargetingExpressions != null && approvedAd.TargetingExpressions.Count > 0)
                {
                    var toBeContinued = false;
                    foreach (var targetExpr in approvedAd.TargetingExpressions)
                    {
                        var isTarget = TargetEligibility(targetExpr.TargetingBody, targetingParam);

                        var require = targetExpr.TargetingAction == TargetingAction.Require;
                        if ((!isTarget && require) || (isTarget && !require))// only support Require or Exclude
                        
                        {
                            toBeContinued = true;
                            break;
                        }

                        //if (targetExpr.TargetingAction == TargetingAction.Exclude)
                        //{
                        //    toBeContinued = true;
                        //    break;
                        //}
                        }

                    if (toBeContinued) continue;

                    results.Add(approvedAd);
                }
            }

            return results;
        }

        private bool TargetEligibility(string xmlExpr, TargetingParam targetingParam)
        {
            // If we don't have Eligibility so we use the results of previous steps => return true
            if (string.IsNullOrEmpty(xmlExpr)) return true;

            var xmlExprElement = GetXmlElement(xmlExpr);
            return ProcessTerm(xmlExprElement, targetingParam);
        }

        private bool ProcessTerm(XmlElement xmlEle, TargetingParam targetingParam)
        {
            if (xmlEle.Name.ToLower() == TermName)
            {
                bool? result = null;

                for (int i = 0; i < xmlEle.ChildNodes.Count; i++)
                {
                    bool temp;
                    if (xmlEle.Attributes[NodeType].Value == NodeTypeAndValue)
                    {
                        // only continue when result is nul or true
                        if (result == null || result.Value)
                        {
                            if (!result.HasValue) result = true;

                            if (xmlEle.ChildNodes[i].Attributes != null && xmlEle.ChildNodes[i].Attributes.Count > 0 &&
                                xmlEle.ChildNodes[i].Name.ToLower() == TermName)
                            {
                                // loop 
                                temp = ProcessTerm((XmlElement)xmlEle.ChildNodes[i], targetingParam);
                                result = result.Value && temp;
                            }
                            else
                            {
                                temp = ProcessSingleNode((XmlElement)xmlEle.ChildNodes[i], targetingParam);
                                result = result.Value && temp;
                            }
                        }
                    }
                    else
                    {
                        // only continue when result is false
                        if (result == null || !result.Value)
                        {
                            if (!result.HasValue) result = false;

                            if (xmlEle.ChildNodes[i].Attributes != null && xmlEle.ChildNodes[i].Attributes.Count > 0 &&
                                xmlEle.ChildNodes[i].Name.ToLower() == TermName)
                            {
                                // loop 
                                temp = ProcessTerm((XmlElement)xmlEle.ChildNodes[i], targetingParam);
                                result = result.Value || temp;
                            }
                            else
                            {
                                temp = ProcessSingleNode((XmlElement)xmlEle.ChildNodes[i], targetingParam);
                                result = result.Value || temp;
                            }
                        }
                    }
                }

                return result.Value;
            }
            else // single node
            {
                return ProcessSingleNode(xmlEle, targetingParam);
            }
        }

        private bool ProcessSingleNode(XmlElement node, TargetingParam targetingParam)
        {
            var compareValue = node.LastChild.InnerText;
            var operatorCS = node.Attributes[OperAttr].Value;
            var field = string.Empty;
            if (node.FirstChild.Attributes != null && node.FirstChild.Attributes[IdAttr] != null)
                field = node.FirstChild.Attributes[IdAttr].Value;
            if (operatorCS == OperAttrEqualValue || operatorCS == OperAttrContainsValue)
            {
                switch (field)
                {
                    case TargetMarketTypeFieldName:
                    return IsTargetMarketType(compareValue, targetingParam.MarketType);
                    case TargetAudienceFieldName:
                        return IsTargetAudienceType(compareValue, targetingParam.AudienceType, operatorCS);
                    case TargetProductTypeFieldName:
                        return IsTargetProductType(compareValue, targetingParam.ProductType, operatorCS);
                    case TargetPigFieldName:
                        return IsTargetPig(compareValue, targetingParam.PIG, operatorCS);
                    case TargetSiteBrandingFieldName:
                    return IsTargetSiteBranding(compareValue, targetingParam.SiteBranding);
                    case TargetOrgIdFieldName:
                    return IsTargetOrgId(compareValue, targetingParam.OrgId);
                    case TargetOrgNameFieldName:
                    return IsTargetOrgName(compareValue, targetingParam.OrgName);
                    default:
                    return false;
                }
            }
            else if (operatorCS == OperAttrNotEqualValue || operatorCS == OperAttrNotContainsValue)
            {
                switch (field)
                {
                    case TargetMarketTypeFieldName:
                    return !IsTargetMarketType(compareValue, targetingParam.MarketType);
                    case TargetAudienceFieldName:
                        return !IsTargetAudienceType(compareValue, targetingParam.AudienceType, operatorCS);
                    case TargetProductTypeFieldName:
                        return !IsTargetProductType(compareValue, targetingParam.ProductType, operatorCS);
                    case TargetPigFieldName:
                        return !IsTargetPig(compareValue, targetingParam.PIG, operatorCS);
                    case TargetSiteBrandingFieldName:
                    return !IsTargetSiteBranding(compareValue, targetingParam.SiteBranding);
                    case TargetOrgIdFieldName:
                    return !IsTargetOrgId(compareValue, targetingParam.OrgId);
                    case TargetOrgNameFieldName:
                    return !IsTargetOrgName(compareValue, targetingParam.OrgName);
                    default:
                    return false;
                }
            }
            else
            {
                // expression case
                if (operatorCS == OperAttrIsTrueValue || operatorCS == OperAttrIsFalseValue)
                {
                    if (!string.IsNullOrEmpty(field))
                    {
                        var expressionId = int.Parse(field);
                        var globalExpr = MarketingDAOManager.Instance.GetExpressionBodyById(expressionId);
                        if (!string.IsNullOrEmpty(globalExpr))
                        {
                            var xmlExpr = GetXmlElement(globalExpr);
                            var result = ProcessTerm(xmlExpr, targetingParam);
                            return operatorCS == OperAttrIsTrueValue ? result : !result;
                        }
                    }
                }
            }

            return false;
        }

        public XmlElement GetXmlElement(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }

        private static bool IsTargetProductType(string productType, string targetProductType, string operatorCS)
        {
            if (operatorCS == OperAttrEqualValue || operatorCS == OperAttrNotEqualValue)
                return targetProductType.ToLower().Equals(productType.ToLower());

            return targetProductType.ToLower().Contains(productType.ToLower());
        }

        private static bool IsTargetAudienceType(string audi, string targetAudienceType, string operatorCS)
        {
            if (operatorCS == OperAttrEqualValue || operatorCS == OperAttrNotEqualValue)
                return targetAudienceType.ToLower().Equals(audi.ToLower());

            return targetAudienceType.ToLower().Contains(audi.ToLower());
        }

        private static bool IsTargetMarketType(string mt, string targetMarketType)
        {
            return (String.Compare(targetMarketType, mt, StringComparison.Ordinal) == 0);
        }

        private static bool IsTargetSiteBranding(string siteBranding, string targetBranding)
        {
            return (String.Compare(targetBranding, siteBranding, StringComparison.Ordinal) == 0);
        }

        private static bool IsTargetPig(string pig, string targetPig, string operatorCS)
        {
            if (operatorCS == OperAttrEqualValue || operatorCS == OperAttrNotEqualValue)
                return targetPig.ToLower().Equals(pig.ToLower());
            return targetPig.ToLower().Contains(pig.ToLower()); //||
            //(String.Compare(pig.ToLower(), TargetingContextAll.ToLower(), StringComparison.Ordinal) == 0);
        }

        private static bool IsTargetOrgId(string orgId, string targetOrgId)
        {
            return targetOrgId.ToLower().Contains(orgId.ToLower());
        }

        private static bool IsTargetOrgName(string orgName, string targetOrgName)
        {
            return targetOrgName.ToLower().Contains(orgName.ToLower());
        }

        public Collection<CampaignAdItem> ConvertListToCollection(List<MarketingApprovedAd> approvedAds)
        {
            var results = new Collection<CampaignAdItem>();

            foreach (var ad in approvedAds)
            {
                var item = new CampaignAdItem {Id = ad.AdIndex, AdName = ad.AdName};

                results.Add(item);
            }
            return results;
        }
    }
}
