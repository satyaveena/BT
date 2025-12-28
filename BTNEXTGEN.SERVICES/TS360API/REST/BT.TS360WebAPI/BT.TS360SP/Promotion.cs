using BT.TS360API.Marketing.DataAccess;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360SP
{
    public class Promotion : CMListBase<PromotionItem>
    {
        string RequestDomainName { get; set; }

        public Promotion(string requestDomainName)
            : base("Promotion")
        {
            HasAdName = true;
            HasDisplayOrder = true;
            RequestDomainName = requestDomainName;
        }

        protected override string GetListName()
        {
            return "Promotion";
        }
        protected override string AddWhereCamlTo(string condition)
        {
            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;

            if (string.IsNullOrEmpty(condition))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + condition + startDateOr + CMConstants.AND_TAG_CLOSE;
        }
        protected override void Refine(IList<PromotionItem> items)
        {
            foreach (PromotionItem item in items)
            {
                //item.ImageUrl = ContentManagementHelper.RefineUrlProtocolToContextProtocol(item.ImageUrl);
                item.ImageUrl = ContentManagementHelper.GetRelativePath(item.ImageUrl, RequestDomainName);

                var BtKeyListString = "";
                //var listBtKeys = ProductCatalogDAO.Instance.GetBTKeysFromCategoryName(item.PromotionCode);
                var listBtKeys = CsProductCatalogDAO.Instance.GetBTKeysFromCategoryName(item.PromotionCode);

                if (listBtKeys != null && listBtKeys.Count > 0)
                {
                    foreach (string btkey in listBtKeys)
                    {
                        BtKeyListString += string.Format("{0}|", btkey);
                    }
                    BtKeyListString.TrimEnd('|');
                }
                item.BtKeyList = BtKeyListString; //CatalogHelper.BuildBtKeyString(item.PromotionCode);                
                item.ItemCurrenceURL = RefinePromoTargetPage((item));
            }
        }

        private string RefinePromoTargetPage(PromotionItem promotionItem)
        {
            if (String.IsNullOrEmpty(promotionItem.PromotionCode) || String.IsNullOrEmpty(promotionItem.BtKeyList))
            {
                return String.Format("{0}?promotionId={1}", SiteUrl.PromotionDetails, promotionItem.Id);
            }
            //Fix TFS #1151
            return String.Format("{0}?promotionId={1}", SiteUrl.PromotionProducts, promotionItem.Id);
        }

        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id
                       , item => item["Title"]
                       ,item => item["AdName"]
                       , item => item["ImageUrl"]
                       , item => item["SummaryText"]
                       , item => item["DetailText"]
                       , item => item["DisplayOrder"]
                       , item => item["PromotionCode"]
                       , item => item["ImageWebtrendsTag"]
                       , item => item["ButtonWebtrendsTag"]
                       ));
        }
    }
}
