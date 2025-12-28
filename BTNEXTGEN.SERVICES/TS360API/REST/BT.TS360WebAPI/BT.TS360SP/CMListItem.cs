using System;
using BT.TS360Constants;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class CMListItem : ICMListItem
    {
        public MarketType? MarketType;
        public ProductType? ProductType;
        public AudienceType? AudienceType;

        public int Id { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Title { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string ContentOwner { get; set; }
        public ItemStatus? ItemStatus;

        public string AdName;
        public bool IsDefault;

        public void Initialize(ListItem item)
        {
            SPListItemMapping(item);
        }

        public string GetAdName()
        {
            return AdName;
        }
        public virtual void SPListItemMapping(ListItem item)
        {
            Id = item.Id;
            //if (item.Fields.ContainsField("Title"))
            Title = item["Title"] as string;
            //if (item.Fields.ContainsField("PublishedDate"))
            //    PublishedDate = (DateTime?)item["PublishedDate"];
            //if (item.Fields.ContainsField("ContentOwner"))
            //{
            //    var userText = item["ContentOwner"] as string;
            //    if (!string.IsNullOrEmpty(userText))
            //        ContentOwner = (new SPFieldUserValue(item.Web, userText)).User.Name;
            //    else ContentOwner = string.Empty;
            //}
            //if (item.Fields.ContainsField("ExpirationDate"))
            //    ExpirationDate = (DateTime?)item["ExpirationDate"];
            //if (item.Fields.ContainsField("AdName"))
            //{
            //    AdName = item["AdName"] as string;
            //}

            //if (item.Fields.ContainsField("IsDefault"))
            //{
            //    IsDefault = (bool)item["IsDefault"];
            //}

            //SetValueToItemStatus(item);
        }

        /// <summary>
        /// Return true if value is Yes, otherwise return false.
        /// </summary>
        /// <param name="value">Yes/No</param>
        /// <returns></returns>
        protected bool ToBoolField(string value)
        {
            if (string.Compare(value, "Yes", true) == 0)
            {
                return true;
            }
            return false;
        }

        protected double? ConvertDisplayOrder(string value)
        {
            double? result;
            try
            {
                if (string.IsNullOrEmpty(value))
                    result = null;
                else
                    result = Convert.ToDouble(value);
            }
            catch (FormatException)
            {
                result = null;
            }
            catch (OverflowException)
            {
                result = null;
            }
            return result;
        }
        //private void SetValueToItemStatus(SPListItem item)
        //{
        //    if (!item.Fields.ContainsField("ItemStatus"))
        //    {
        //        this.ItemStatus = ItemStatus.Published;ItemStatus.
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(item["ItemStatus"] as string))
        //    {
        //        this.ItemStatus = ContentManagement.ItemStatus.None;
        //        return;
        //    }
        //    var itemValues = new SPFieldMultiChoiceValue(item["ItemStatus"].ToString());
        //    for (int i = 0; i < itemValues.Count; i++)
        //    {
        //        string itemstatus = itemValues[i];
        //        switch (itemstatus)
        //        {
        //            case "Draft":
        //                this.ItemStatus = ContentManagement.ItemStatus.Draft;
        //                break;
        //            case "Approved":
        //                this.ItemStatus = ContentManagement.ItemStatus.Approved;
        //                break;
        //            case "Published":
        //                this.ItemStatus = ContentManagement.ItemStatus.Published;
        //                break;
        //            case "Expired":
        //                this.ItemStatus = ContentManagement.ItemStatus.Expired;
        //                break;
        //            default:
        //                this.ItemStatus = ContentManagement.ItemStatus.Invalid;
        //                break;
        //        }
        //    }
        //}

        protected string GetUrlFromFieldUrlValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = ContentManagementHelper.RefineUrlFromAuthToInternet(value);

                return value;
            }

            return string.Empty;
        }
        //protected DocumentType? ToDocumentType(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return DocumentType.None;
        //    switch (value)
        //    {
        //        case "Terms of Service":
        //            return DocumentType.TermsOfService;
        //        case "System Requirements":
        //            return DocumentType.SystemRequirements;
        //        case "Privacy Policy":
        //            return DocumentType.PrivacyPolicy;
        //        case "Help":
        //            return DocumentType.Help;
        //        //case "ComingSoonCarousel":
        //        //    return DocumentType.ComingSoonCarousel;
        //        //case "WhatsHot":
        //        //    return DocumentType.WhatsHot;
        //        //case "InTheNews":
        //        //    return DocumentType.InTheNews;
        //        case "RetailArea":
        //            return DocumentType.RetailArea;
        //        case "LibraryArea":
        //            return DocumentType.LibraryArea;
        //        case "FeaturedTitles":
        //            return DocumentType.FeaturedTitles;
        //        case "Testimonial":
        //            return DocumentType.Testimonial;
        //        case "AdvancedSearchTips":
        //            return DocumentType.AdvancedSearchTips;
        //        //case "Popular B&T eLists":
        //        //    return DocumentType.PopularBTeLists;
        //        default:
        //            return DocumentType.Invalid;
        //    }
        //}
        //protected Category ToCategory(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return Category.None;
        //    switch (value)
        //    {
        //        case "(1) Category1":
        //            return Category._1Category1;
        //        case "(2) Category2":
        //            return Category._2Category2;
        //        case "(3) Category3":
        //            return Category._3Category3;
        //        default:
        //            return Category.Invalid;
        //    }
        //}
        //protected Priority ToPriority(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return Priority.None;
        //    switch (value)
        //    {
        //        case "(1) High":
        //            return Priority._1High;
        //        case "(2) Normal":
        //            return Priority._2Normal;
        //        case "(3) Low":
        //            return Priority._3Low;
        //        default:
        //            return Priority.Invalid;
        //    }
        //}
        //protected TaskStatus ToTaskStatus(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return TaskStatus.None;
        //    switch (value)
        //    {
        //        case "Not Started":
        //            return TaskStatus.NotStarted;
        //        case "In Progress":
        //            return TaskStatus.InProgress;
        //        case "Completed":
        //            return TaskStatus.Completed;
        //        case "Deferred":
        //            return TaskStatus.Deferred;
        //        case "Waiting on someone else":
        //            return TaskStatus.WaitingOnSomeoneElse;
        //        default:
        //            return TaskStatus.Invalid;
        //    }
        //}

        protected PromotionFolder ToPromotionFolder(string value)
        {
            if (string.IsNullOrEmpty(value))
                return PromotionFolder.None;
            switch (value)
            {
                case "Promotion 1":
                    return PromotionFolder.Promotion1;
                case "Promotion 2":
                    return PromotionFolder.Promotion2;
                case "Promotion 3":
                    return PromotionFolder.Promotion3;
                case "Promotion 4":
                    return PromotionFolder.Promotion4;
                case "Promotion 5":
                    return PromotionFolder.Promotion5;
                case "Promotion 6":
                    return PromotionFolder.Promotion6;
                case "Promotion 7":
                    return PromotionFolder.Promotion7;
                case "Promotion 8":
                    return PromotionFolder.Promotion8;
                case "Promotion 9":
                    return PromotionFolder.Promotion9;
                case "Promotion 10":
                    return PromotionFolder.Promotion10;
                default:
                    return PromotionFolder.Invalid;
            }
        }

        //protected TopRelease ToTopRelease(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return TopRelease.None;
        //    switch (value)
        //    {
        //        case "Yes":
        //            return TopRelease.Yes;
        //        case "No":
        //            return TopRelease.No;
        //        default:
        //            return TopRelease.Invalid;
        //    }
        //}
        //protected AdvertisementFolder ToAdvertisementFolder(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return AdvertisementFolder.None;
        //    switch (value)
        //    {
        //        case "Advertisement 1":
        //            return AdvertisementFolder.Advertisement1;
        //        case "Advertisement 2":
        //            return AdvertisementFolder.Advertisement2;
        //        default:
        //            return AdvertisementFolder.Invalid;
        //    }
        //}
        //protected AdvertisementFolder0 ToAdvertisementFolder0(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return AdvertisementFolder0.None;
        //    switch (value)
        //    {
        //        case "Advertisement 1":
        //            return AdvertisementFolder0.Advertisement1;
        //        case "Advertisement 2":
        //            return AdvertisementFolder0.Advertisement2;
        //        case "Advertisement 3":
        //            return AdvertisementFolder0.Advertisement3;
        //        default:
        //            return AdvertisementFolder0.Invalid;
        //    }
        //}
        //protected Priority0 ToPriority0(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return Priority0.None;
        //    switch (value)
        //    {
        //        case "High":
        //            return Priority0.High;
        //        case "Medium":
        //            return Priority0.Medium;
        //        case "Low":
        //            return Priority0.Low;
        //        default:
        //            return Priority0.Invalid;
        //    }
        //}

        protected Mode ToMode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Mode.None;
            switch (value)
            {
                case "Turn On":
                    return Mode.TurnOn;
                case "Turn Off":
                    return Mode.TurnOff;
                default:
                    return Mode.Invalid;
            }
        }
    }
}
