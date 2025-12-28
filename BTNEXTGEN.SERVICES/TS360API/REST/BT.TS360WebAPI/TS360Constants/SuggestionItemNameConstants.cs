namespace BT.TS360Constants
{
    public sealed class SuggestionItemNameConstants
    {
        /// <summary>
        /// Special search field for Basic Search
        /// </summary>
        public const string QueryTerms = "ngqueryterms";

        public const string QueryTermsRank = "autosuggest"; //"ngquerytermsrank";

        public static string AutoSuggestCollection
        {
            get
            {
                //var autoSuggestCollection = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.Auto_Suggest_Collection);
                //return autoSuggestCollection != null ? autoSuggestCollection.Value : string.Empty;

                return AppSettings.AutoSuggestCollection;
            }
        }
    }
    public sealed class SearchViewItemConstants
    {
        public static string SearchViewItem
        {
            get
            {
                //var searchViewItem = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.Search_View_Item);
                //return searchViewItem != null ? searchViewItem.Value : string.Empty;
                return AppSettings.SearchViewItem;
            }
        }
    }
}
