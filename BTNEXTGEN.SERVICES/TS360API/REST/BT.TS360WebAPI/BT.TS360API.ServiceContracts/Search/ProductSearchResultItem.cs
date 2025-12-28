using System;
using System.Collections.Generic;
using System.Text;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.Search
{
    public class ProductSearchResultItem : FastSearchResultItem
    {
        public string UserId { get; set; } //TODO: should get from SiteContext.Current.UserId

        private FlagObject _flagObject;

        public string Promotion { get; set; }
        public decimal DiscountPrice { get; set; }

        public string ContentIndicatorText
        {
            get; // TODO: move to outside: SearchHelper.GetContentIndicatorText 
            set;
        } 
        //{
        //    if (this._flagObject == null)
        //        this._flagObject = ProductSearchController.GetFlagObject(UserId); // ProductSearchController.GetFlagObject(SiteContext.Current.UserId);
        //    //
        //    var hasMuze = false;
        //    var hasToc = false;
        //    if (_flagObject != null)
        //    {
        //        hasMuze = _flagObject.ShowMuze;
        //        hasToc = _flagObject.ShowToc;
        //    }
        //    //var url = Request.Url.GetLeftPart(UriPartial.Authority);
        //    var prodContent = new ProductContent
        //    {
        //        HasAnnotation = this.HasAnnotations,
        //        HasExcerpts = this.HasExcerpt,
        //        HasReturnKey = this.HasReturn
        //    };
        //    if (hasMuze && this.ProductType != ProductTypeConstants.Book)
        //        prodContent.HasMuze = this.HasMuze;
        //    //
        //    prodContent.HasReviews = this.HasReview;
        //    //
        //    if (hasToc)
        //        prodContent.HasTOC = this.HasToc;
        //    //
        //    var content = SearchHelper.CombineContentIndicator(prodContent, this.BTKey, string.Empty);
        //    return content;
        //}
        public string InventoryStatus { get; set; }
        public string MultiPack { get; set; }
        public string DiscountText { get; set; }
        public decimal DiscountPercent { get; set; }
        public int Quantity { get; set; }
        public DateTime AddToCartDate { get; set; }
        public OnHandInventory Inventory { get; set; }
        public string Bib { get; set; }
        public string POLine { get; set; }
        public ISBNLookUpLink ISBNLookUpLink
        {
            get; // TODO: move to outside: ProductSearchController.CreateIsbnLookupLink 
            set;
            //get { return ProductSearchController.CreateIsbnLookupLink(this.ISBN, this.ISBN10); }
        }

        public string BasketOriginalEntryId { get; set; }

        public bool IsOriginalEntryItem
        {
            get
            {
                return !string.IsNullOrEmpty(BasketOriginalEntryId);
            }
        }

        public string AuthorText
        {
            get { return this.CreateAuthorText(this.ResponsiblePartys); }
        }

        /// <summary>
        /// LineItemID for new BTGridNote
        /// </summary>
        public string LineItemID { get; set; }

        public string ListPriceText
        {
            get
            {
                if (ListPrice == -1)
                {
                    return CommonConstants.NALiteral;
                }
                return GetCurrencyFormat(ListPrice);
            }
        }

        public string DiscountPriceText
        {
            get
            {
                if (DiscountPrice == -1)
                {
                    return CommonConstants.NALiteral;
                }
                return GetCurrencyFormat(DiscountPrice);
            }
        }

        public bool IsLanguageExceedMaxLength
        {
            get
            {
                if (LanguageLiteral == null) return false;
                return LanguageLiteral.Length > 20;
            }
        }

        public string LanguageLiteralForUI
        {
            get
            {
                if (string.IsNullOrEmpty(LanguageLiteral) ||
                    string.Compare(LanguageLiteral, CommonConstants.DefaultLanguageLiteral, StringComparison.OrdinalIgnoreCase) == 0) return "";
                //
                var literal = LanguageLiteral.ToLower();
                return IsLanguageExceedMaxLength ? string.Format("{0}...", literal.Substring(0, 17)) : literal;
            }
        }

        public string ProductFormatForUI
        {
            get
            {
                var productFormat = string.Empty;

                if (string.Compare(FormatClass, CommonConstants.ICON_MAKERSPACE, StringComparison.OrdinalIgnoreCase) == 0)
                    productFormat = ProductFormatConstants.Book_Makerspace;
                else
                    productFormat = ProductFormat;

                if (!string.IsNullOrEmpty(ChildrenFormat))
                    productFormat += "\\ " + ChildrenFormat;

                return productFormat;
            }

        }

        public bool HasDiversityClassification { get; set; }
        public List<string> ClassificationName { get; set; }


        /// <summary>
        /// Initializes a new instance of the ProductSearchResultItem class. Default constructor.
        /// </summary>
        public ProductSearchResultItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProductSearchResultItem class.
        /// </summary>
        /// <param name="document">Search result document</param>
        public ProductSearchResultItem(BT.FSIS.Document document)
            : base(document)
        {
        }

        #region private

        private string CreateAuthorText(string authorsStr)
        {
            var result = new StringBuilder();
            if (!String.IsNullOrEmpty(authorsStr))
            {
                string[] authors = authorsStr.Split('#');
                if (authors.Length > 0)
                    result.Append(authors[0]);
                for (int i = 1; i < authors.Length; i++)
                {
                    result.Append("/ " + authors[i]);
                }
            }
            return result.ToString();
        }

        private string GetCurrencyFormat(string value)
        {
            try
            {
                return GetCurrencyFormat(Decimal.Parse(value));
            }
            catch (Exception)
            {
                return value;   //Don't need to format if it's exception
            }
        }

        private string GetCurrencyFormat(decimal value)
        {
            try
            {
                // round currency value
                return value.ToString("c");
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        #endregion
    }
}
