using System.Collections.Generic;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts
{
    public class ItemDataContract
    {
        public string ItemKey { get; set; }
        public string ItemValue { get; set; }
    }
    public class SiteTermItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public string SearchValue { get; set; }
        public List<SiteTermItem> Children { get; set; }
    }
    public class SiteTermCategory
    {
        public string Category { get; set; }
        public List<SiteTermItem> SiteTermObjectList { get; set; }
    }

    public class SiteTermResponse
    {
        public SiteTermCategory Data { get; set; }

        public AppServiceStatus Status { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public SiteTermResponse()
        {
            Status = AppServiceStatus.Success;
        }
    }

    public class ItemData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemData"/> class.
        /// </summary>
        /// <param name="itemDataValue"> The ItemData value</param>
        /// <param name="itemDataText"> The SitItemDataeTerm text</param>
        public ItemData(string itemDataValue, string itemDataText)
        {
            ItemDataValue = itemDataValue;
            ItemDataText = itemDataText;
            ItemDataDescription = string.Empty;
        }

        public ItemData(string itemDataValue, string itemDataText, string itemDataDescription)
        {
            ItemDataValue = itemDataValue;
            ItemDataText = itemDataText;
            ItemDataDescription = itemDataDescription;
        }

        
        public string ItemDataText { get; set; }

        
        public string ItemDataValue { get; set; }

        
        public string ItemDataDescription { get; set; }

        
        public bool IsChild { get; set; }

        public ItemData()
        {
            // TODO: Complete member initialization
        }
    }
}
