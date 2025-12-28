//-----------------------------------------------------------------------
// <copyright file="ItemData.cs" company="Microsoft">
//     Copyright © Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary> Item Data </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BT.TS360API.ExternalDataSendService.Models
{
    [DataContract]
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

        public ItemData(string itemDataValue, string itemDataText, string itemDataSearchValue, List<ItemData> childItems)
        {
            ItemDataValue = itemDataValue;
            ItemDataText = itemDataText;            
            ItemDataSearchValue = itemDataSearchValue;
            ChildItems = childItems;
        }

        public ItemData(string itemDataValue, string itemDataText, string itemDataDescription, string itemDataSearchValue, List<ItemData> childItems)
            : this(itemDataValue, itemDataText, itemDataDescription)
        {
            ItemDataSearchValue = itemDataSearchValue;
            ChildItems = childItems;
        }
        [DataMember]
        public string ItemDataText { get; set; }

        [DataMember]
        public string ItemDataSearchValue { get; set; }

        [DataMember]
        public string ItemDataValue { get; set; }

        [DataMember]
        public string ItemDataDescription { get; set; }

        [DataMember]
        public bool IsChild { get; set; }

        public List<ItemData> ChildItems { get; set; }
        public bool IsChecked { get; set; }

        public string ParentId { get; set; }

        public ItemData()
        {
            // TODO: Complete member initialization
        }
    }

    public static class SiteTermFilter
    {
        public static bool FormatFilterRetailType(ItemData data)
        {
            List<string> RemovedItem = new List<string>();
            RemovedItem.Add("ebookDownloadableAudio");
            RemovedItem.Add("ebookDigitalDownloadAndOnline");
            RemovedItem.Add("ebookDigitalOnline");
            RemovedItem.Add("ebookDigitalDownload");
            return !RemovedItem.Contains(data.ItemDataValue);
        }

        public static bool FormatFilterLibraryType(ItemData data)
        {
            List<string> RemovedItem = new List<string>();
            RemovedItem.Add("cassette");
            RemovedItem.Add("ebookDigitalDownloadAndOnline");
            RemovedItem.Add("ebookDigitalOnline");
            return !RemovedItem.Contains(data.ItemDataValue.Trim().ToLower());
        }
    }
}