using System;

namespace BT.TS360API.ServiceContracts
{
    public class CampaignAdItem
    {
        public int Id { get; set; }
        public string AdName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class BtKeyCatalogObject
    {
        public BtKeyCatalogObject()
        {
        }

        public BtKeyCatalogObject(string btKey, string catalogName, string baseCatalogName)
        {
            BTKey = btKey;
            CatalogName = catalogName;
            BaseCatalogName = baseCatalogName;
        }

        public string BTKey { get; set; }
        public string CatalogName { get; set; }
        public string BaseCatalogName { get; set; }
    }

    public class BtKeyParentCategoryObject
    {
        public BtKeyParentCategoryObject()
        {
        }

        public BtKeyParentCategoryObject(string btKey, string catalogName, string parentCat, string primaryParentCat)
        {
            BTKey = btKey;
            CatalogName = catalogName;
            ParentCategory = parentCat;
            PrimaryParentCategory = primaryParentCat;
        }

        public string BTKey { get; set; }
        public string CatalogName { get; set; }
        public string ParentCategory { get; set; }
        public string PrimaryParentCategory { get; set; }
    }

    public class BtKeyDiscountObject
    {
        public BtKeyDiscountObject()
        {
        }

        public BtKeyDiscountObject(string btKey, string discountName)
        {
            BTKey = btKey;
            DiscountName = discountName;
        }

        public string BTKey { get; set; }
        public string DiscountName { get; set; }
    }
}
