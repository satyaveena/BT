using System.Collections.Generic;
using System.Linq;
using BT.FSIS;
using BT.TS360API.Common.Search;

namespace BT.TS360API.ServiceContracts.Search
{
    public class ProductSearchResults : SearchResults<ProductSearchResultItem>
    {
        private SearchResults searchResults;

        /// <summary>
        /// Initializes a new instance of the ProductSearchResults class.
        /// </summary>
        public ProductSearchResults()
        {
            this.Items = new List<ProductSearchResultItem>();
        }

        /// <summary>
        /// Initializes a new instance of the ProductSearchResults class.
        /// </summary>
        /// <param name="searchResults">FAST search result</param>
        public ProductSearchResults(BT.FSIS.SearchResults searchResults)
        {
            this.searchResults = searchResults;
            this.InitializeProperties();
        }

        /// <summary>
        /// Gets navigator collection
        /// </summary>
        public List<Navigator> Navigators
        {
            get
            {
                return (searchResults != null) ? searchResults.Navigators : null;
            }
            set
            {
                if (searchResults != null)
                {
                    searchResults.Navigators = value;
                }
            }
        }

        public List<Cluster> Clusters
        {
            get
            {
                return (searchResults != null) ? searchResults.Clusters : null;
            }
            set
            {
                if (searchResults != null)
                {
                    searchResults.Clusters = value;
                }
            }
        }

        ///<sumary>
        ///suggest phrase
        ///</sumary>

        public string SuggestPhrase
        {
            get;
            set;
        }

        /// <summary>
        /// Initialize properties
        /// </summary>
        private void InitializeProperties()
        {
            if (this.searchResults != null)
            {
                this.TotalRowCount = this.searchResults.DocumentCount;
                this.SuggestPhrase = this.searchResults.SpellCheckSuggestion;
                if (this.searchResults.ResultDocuments != null)
                {
                    if (this.Items == null)
                    {
                        this.Items = new List<ProductSearchResultItem>();
                    }
                    else
                    {
                        this.Items.Clear();
                    }

                    foreach (var document in this.searchResults.ResultDocuments)
                    {
                        this.Items.Add(new ProductSearchResultItem(document));
                    }
                }
            }
        }

        public ProductSearchResultItem GetProductSearchResultItem(string btKey)
        {
            return this.Items.Select(i => i).Where(i => i.BTKey == btKey).FirstOrDefault();
        }

        public string Errors { get { return (searchResults != null) ? searchResults.Errors : string.Empty; } }

        public bool HasErrors { get { return (searchResults != null) ? searchResults.HasErrors : false; } }
    }
}
