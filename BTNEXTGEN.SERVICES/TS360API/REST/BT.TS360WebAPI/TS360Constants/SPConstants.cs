using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360Constants
{
    public sealed class CMConstants
    {
        public const string ManagedContentTypeName = "Managed Content";
        public const string UnfilterdManagedContentTypeName = "Unfiltered Managed Content";
        public const string ManagedDocumentContentTypeName = "Managed Asset or Document";
        public const string ExpirationDateFieldName = "ExpirationDate";
        public const string ArchiveDate = "ArchiveDate";
        public const string ApprovalStatusFieldName = "PublishedStatus";
        public const string SPApprovalWFStatusFieldName = "_ModerationStatus";
        public const string WorkflowStatusFieldName = "ItemStatus";
        public const string HTTP = "http";
        public const string HTTPS = "https";
        public const string Suggestion_Items_Limit_Default = "10";
        public const string Suggestion_Delay_Time_Default = "300";

        public const string AND_TAG_OPEN = "<And>";
        public const string AND_TAG_CLOSE = "</And>";
        public const string EQ_TAG_OPEN = "<Eq>";
        public const string EQ_TAG_CLOSE = "</Eq>";
        public const string GT_TAG_OPEN = "<Gt>";
        public const string GT_TAG_CLOSE = "</Gt>";
        public const string GEQ_TAG_OPEN = "<Geq>";
        public const string GEQ_TAG_CLOSE = "</Geq>";
        public const string LEQ_TAG_OPEN = "<Leq>";
        public const string LEQ_TAG_CLOSE = "</Leq>";
        public const string CON_TAG_OPEN = "<Contains>";
        public const string CON_TAG_CLOSE = "</Contains>";
        public const string IsNull_TAG_OPEN = "<IsNull>";
        public const string IsNull_TAG_CLOSE = "</IsNull>";
        public const string IsNotNull_TAG_OPEN = "<IsNotNull>";
        public const string IsNotNull_TAG_CLOSE = "</IsNotNull>";
        public const string OR_TAG_OPEN = "<Or>";
        public const string OR_TAG_CLOSE = "</Or>";
        public const string QUERY_TAG_OPEN = "<Query>";
        public const string QUERY_TAG_CLOSE = "</Query>";
        public const string WHERE_TAG_OPEN = "<Where>";
        public const string WHERE_TAG_CLOSE = "</Where>";
        public const string ORDERBY_TAG_OPEN = "<OrderBy>";
        public const string ORDERBY_TAG_CLOSE = "</OrderBy>";

        public const string InternetSiteUrl = "InternetSiteURL";
        public const string AuthenticatedSiteUrl = "AuthenticatedSiteURL";
        //public const string InternetSiteUrlDefaultValue = "http://ts360dev.baker-taylor.com";
        //public const string AuthenticatedSiteUrlDefaultValue = "http://ts360devauth.baker-taylor.com";
        public const string SearchTypeAheadDemandbucket = "SearchTypeAheadDemandBucket";
        public const string Demandbucketname = "DemandBucketName";
        public const string Demandhitcountrange = "DemandHitCountRange";
        public const string Fastboostvalue = "FASTBoostValue";
        public const string DemandBucketSettingsCacheKey = "__SearchTypeAheadDemandBucketSettingsCacheKey";
        public const string DemandBucketCachingMinutesKey = "DemandBucketCachingMinutes";

        public const string DefaultFieldNames = "<FieldRef Name=\'ID\' /><FieldRef Name=\'Title\' /><FieldRef Name=\'ContentOwner\' />" +
                                                "<FieldRef Name=\'PublishedDate\' /><FieldRef Name=\'ExpirationDate\' /><FieldRef Name=\'AdName\' />" +
                                                "<FieldRef Name=\'IsDefault\' /><FieldRef Name=\'ItemStatus\' />";
    }
    public sealed class CMFieldNameConstants
    {
        public const string Title = "Title";
        public const string IsDefault = "IsDefault";
        public const string DisplayOrder = "DisplayOrder";
        public const string ExpirationDate = "ExpirationDate";
        public const string ItemStatus = "ItemStatus";
        public const string StartDate = "StartDate";
        public const string ReleaseDate = "ReleaseDate";
        public const string PreOrderDate = "PreOrderDate";
        public const string ProductType = "ProductType";
        public const string BTKeyList = "BTKeyList";
    }

    public sealed class CMListNameConstants
    {
        public const string NewReleaseCalendar = "NewReleaseCalendar";
        public const string NRCFeaturedTitles = "NRCFeaturedTitles";
    }
}
