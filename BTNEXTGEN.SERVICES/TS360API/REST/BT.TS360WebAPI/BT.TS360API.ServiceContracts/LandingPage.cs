using System.Collections.Generic;
using BT.TS360API.ServiceContracts.Search;

namespace BT.TS360API.ServiceContracts
{
    public class LandingPageResponse
    {
    }

    //public class LandingPageRequest: BaseRequest
    //{
    //    public string FolderId { get; set; }
    //}

    public class GetDataForLandingFirstLoadRequest : BaseRequest
    {
        //public bool getTwilight { get; set; }
        public bool isPrimaryCartSet { get; set; }
    }

    public class LandingPageContract
    {
        
        public InTheNewsContract InTheNews { get; set; }

        
        public List<ListCarouselItemData> ListCarouselItem { get; set; }

        
        //public TwilightListContract TwilightList { get; set; }

        
        public string SalesPromoText { get; set; }

        
        public string RailPromoText { get; set; }
    }

    public class InTheNewsContract
    {
        public List<BasketsContract> Baskets { get; set; }
        public bool IsVisible { get; set; }
        public bool WachVideoVisible { get; set; }
        public string NewsVideo { get; set; }
        public string NewsTitle { get; set; }
        public string NewsText { get; set; }
        public string NewsImage { get; set; }
        public string ViewDetail { get; set; }
        public string NewsDocument { get; set; }
    }

    public class BasketsContract
    {
        /// <summary>
        /// Gets or sets the basket id.
        /// </summary>
        /// <value>The basket id.</value>
        
        public string BasketId { get; set; }
        /// <summary>
        /// Gets or sets the name of the basket.
        /// </summary>
        /// <value>The name of the basket.</value>
        
        public string BasketName { get; set; }
        /// <summary>
        /// Gets or sets the basket status.
        /// </summary>
        /// <value>The basket status.</value>
        
        public string BasketStatus { get; set; }
        
        public string FolderName { get; set; }
        
        public string FolderId { get; set; }
        
        public string UserId { get; set; }
        
        public string SelectedBookAccountId { get; set; }
        
        public string SelectedEntertainmentAccountId { get; set; }
        
        public string SelectedBookAccountERPNumber { get; set; }
        
        public string SelectedEntertainmentAccountERPNumber { get; set; }

        
        public string CartOwner { get; set; }
        
        public string PONumber { get; set; }
        
        public string CartNotes { get; set; }
        
        public string TotalLines { get; set; }
        
        public string TotalCarts { get; set; } //use for count the total carts of a folder, include child folders
        
        public string TotalItems { get; set; }
        
        public string TotalListPrice { get; set; }
        
        public string TotalNetPrice { get; set; }
        
        public string TotalCartPrice { get; set; }

        
        public string TotalBookLines { get; set; }
        
        public string TotalBookItems { get; set; }
        
        public string TotalBookListPrice { get; set; }
        
        public string TotalBookNetPrice { get; set; }
        
        public string TotalBookPrice { get; set; }

        
        public string TotalEntertainmentLines { get; set; }
        
        public string TotalEntertainmentItems { get; set; }
        
        public string TotalEntertainmentListPrice { get; set; }
        
        public string TotalEntertainmentNetPrice { get; set; }
        
        public string TotalEntertainmentPrice { get; set; }
        
        public string CartDetailUrl { get; set; }
        
        public string CheckoutUrl { get; set; }
        
        public string Price { get; set; }
        
        public string NoOfItems { get; set; }

        
        public string AccountName { get; set; }
        
        public string AccountId { get; set; }
        
        public string AccountType { get; set; }
        
        public string AccountERPNumber { get; set; }

        
        public string BookAccountId { get; set; }
        
        public string EntertainmentAccountId { get; set; }
        
        public string BookAccountName { get; set; }
        
        public string BookAccountERPNumber { get; set; }
        
        public string EntertainmentAccountERPNumber { get; set; }
        
        public string EntertainmentAccountName { get; set; }
        
        public string BasketNameTruncate { get; set; }
        
        public string FutureOnsaleDate { get; set; }
        /// <summary>
        /// Gets or sets the is archived.
        /// </summary>
        /// <value>The is archived.</value>
        
        public int? IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the is bt cart.
        /// </summary>
        /// <value>The is bt cart.</value>
        
        public int? IsBtCart { get; set; }

        /// <summary>
        /// Gets or sets the is primary.
        /// </summary>
        /// <value>The is primary.</value>
        
        public int? IsPrimary { get; set; }

        
        public List<PricingReturn4ClientObject> LineItems { get; set; }

        
        public int LineItemWithQuantityCount { get; set; }
        
        public int LineItemWithoutQuantityCount { get; set; }
        
        public int LineItemWithNoteCount { get; set; }
        
        public int LineItemWithoutNoteCount { get; set; }
        
        public bool IsProcessing { get; set; }

        
        public string ESupplierID { get; set; }

        
        public List<AccountSummaryContract> Accounts { get; set; }

        
        public bool IsShared { get; set; }

        
        public bool IsActive { get; set; }

        
        public bool IsShowCartDefaultGrid { get; set; }

        
        public bool CanDelete { get; set; }

        
        public bool CanDesignateAsPrimaryCart { get; set; }

        
        public bool HasPermission { get; set; }

        
        public bool HasReviewOrAcquisition { get; set; }

        
        public bool IsPremium { get; set; }

        
        public List<string> RadGridClientIDList { get; set; }

        
        public string TitlesWithoutGrids { get; set; }

        
        public List<DCGridData> DCGridDatas { get; set; }

        
        public int FreezeLevel { get; set; }

        
        public int GridDistributionOption { get; set; }
    }

    public class ListCarouselItemData
    {
        public string BackgroundImageUrl { get; set; }

        
        public string Title { get; set; }

        
        public string PromotionLinkImage { get; set; }

        
        public string PromotionFolder { get; set; }

        
        public string PromotionName { get; set; }

        
        public int Id { get; set; }

        
        public string PromoDesc { get; set; }

        
        public List<JacketCarouselItemData> JacketCarouselItems { get; set; }

        
        public string ListCarouselUrl { get; set; }
    }

    public class JacketCarouselItemData
    {
        
        public int Id { get; set; }

        
        public string BtKey { get; set; }

        
        public string ImageLink { get; set; }
    }

    public class AccountSummaryContract
    {
        public List<BasketsContract> Baskets { get; set; }
        public string AccountType { get; set; }
        public string TotalLines { get; set; }
        public string TotalItems { get; set; }
        public string TotalListPrice { get; set; }
        public string TotalNetPrice { get; set; }
        public string TotalPrice { get; set; }
        public string Discount { get; set; }
    }
}
