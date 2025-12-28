using System.Collections.Generic;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ListCarousel : CMListBase<ListCarouselItem>
    {
        string RequestDomainName { get; set; }
        public ListCarousel(string requestDomainName)
        {
            RequestDomainName = requestDomainName;
            HasIsDefault = true;
            HasAdName = true;
        }
        protected override string GetListName()
        {
            return "ListCarousel";
        }

        protected string QueryCondition(string condition)
        {
            var startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            var startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            var startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;
            if (string.IsNullOrEmpty(condition))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + condition + startDateOr + CMConstants.AND_TAG_CLOSE;
        }

        protected override void Refine(IList<ListCarouselItem> items)
        {
            foreach (ListCarouselItem item in items)
            {
                if (!string.IsNullOrEmpty(item.PromotionLinkImage))
                {
                    item.PromotionLinkImage = ContentManagementHelper.GetRelativePath(item.PromotionLinkImage, RequestDomainName);
                }

                if (!string.IsNullOrEmpty(item.PromoBoxBackgroundImage))
                {
                    item.PromoBoxBackgroundImage = ContentManagementHelper.GetRelativePath(item.PromoBoxBackgroundImage, RequestDomainName);
                }
                if (!string.IsNullOrEmpty(item.ListCarouselURL))
                {
                    item.ListCarouselURL = ContentManagementHelper.GetRelativePath(item.ListCarouselURL, RequestDomainName);
                }
                //if (item.PromotionFolder != null)
                //    item.FolderName = item.PromotionFolder.ToString();// folderName;
            }
        }

        protected override string AddWhereCamlTo(string currentCaml)
        {
            string result = QueryCondition(currentCaml);
            if (!string.IsNullOrEmpty(_camlGetById))
            {
                if (string.IsNullOrEmpty(result))
                    result = _camlGetById;
                else
                    result = CMConstants.AND_TAG_OPEN
                           + _camlGetById + result
                           + CMConstants.AND_TAG_CLOSE;
            }
            return result;
        }

        private string _camlGetById;
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                IsGettingById = true;
                _id = value;
                _camlGetById = ContentManagementHelper.CreateCamlGetById(value.ToString());
            }
        }

        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["AdName"],
                       item => item["PromotionFolder"],
                       item => item["PromotionName"],
                       item => item["PromotionLinkImage"],
                       item => item["PromoBoxBackground"],
                       item => item["PromotionDescription"],
                       item => item["ListCarouselUrl"]
                       ));
        }
    }
}
