using System.Collections.Generic;
using System.Linq;

namespace BT.TS360SP
{
    public static class ListItemDetailSettings
    {
        private const string ListItemDetailConfigVisibleSettingsCacheKey = "__ListItemDetailConfigVisibleSettingsCacheKey";
        private const string ListItemDetailFieldSettingsCacheKey = "__ListItemDetailFieldSettingsCacheKey";

        public static int DemandBucketCachingMinutes
        {
            get
            {
                int demandBucketCachingMinutes = 5;
                var valueFromGlobalConfiguration = AppSettings.DemandBucketCachingMinutes;
                if (!string.IsNullOrEmpty(valueFromGlobalConfiguration))
                {
                    int.TryParse(valueFromGlobalConfiguration, out demandBucketCachingMinutes);
                }
                return demandBucketCachingMinutes;
            }
        }

        public static IList<ListItemDetailConfigurationItem> GetItemDetailConfigVisibleSettings()
        {
            var t = DemandBucketCachingMinutes;
            //var settings = CachingController.Instance.Read(ListItemDetailConfigVisibleSettingsCacheKey) as IList<ListItemDetailConfigurationItem>;
            //if (settings == null)
            //{
                return ContentManagementController.Current.GetItemDetailConfigVisible();
                //CachingController.Instance.Write(ListItemDetailConfigVisibleSettingsCacheKey, settings, DemandBucketCachingMinutes);
            //}
            //return settings;
        }
        public static Dictionary<int, List<ListItemDetailFieldItem>> GetItemDetailFieldSettings()
        {
            //var settings = CacheHelper.Get(ListItemDetailFieldSettingsCacheKey) as Dictionary<int, List<ListItemDetailFieldItem>>;
            //if (settings == null)
            //{
                var settings = new Dictionary<int, List<ListItemDetailFieldItem>>();
                var list = GetItemDetailConfigVisibleSettings();
                if (list != null && list.Count > 0)
                {
                    int[] sectionIds = list.Select(x => x.SectionID.HasValue ? x.SectionID.Value : 1).ToArray();
                    var listFieldId = ContentManagementController.Current.GetItemDetailField(sectionIds);
                    settings = listFieldId.GroupBy(x => x.SectionID.HasValue ? x.SectionID.Value : 1, x => x)
                                        .ToDictionary(group => group.Key, group => group.ToList());
                }
                //CacheHelper.Write(ListItemDetailFieldSettingsCacheKey, settings, DemandBucketCachingMinutes);
            //}
            return settings;
        }
        public static IList<ListItemDetailFieldItem> GetItemDetailField(int key)
        {
            List<ListItemDetailFieldItem> fields = new List<ListItemDetailFieldItem>();
            var fieldSettings = GetItemDetailFieldSettings();
            if (fieldSettings != null && fieldSettings.ContainsKey(key))
                fields = fieldSettings[key];
            return fields;
        }
    }
}
