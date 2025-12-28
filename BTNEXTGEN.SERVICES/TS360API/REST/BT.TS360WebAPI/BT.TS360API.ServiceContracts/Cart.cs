using System;
using System.Collections.Generic;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class Cart
    {
        public string WorkflowTimeZone { get; set; }
        public bool EntertainmentHasGridLine { get; set; }

        #region Constructor
        /// <summary>
        /// Cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="userId"></param>
        public Cart(string cartId, string userId)
        {
            this.CartId = cartId;
            this.UserId = userId;

            CartAccounts = new List<CartAccount>();
        }

        public Cart(string cartId, string userId, string orgId)
            : this(cartId, userId)
        {
            this.OrgId = orgId;
        }
        #endregion
        #region Property
        public string UserId { get; set; }
        public string CartId { get; set; }
        public int NonRankedCount { get; set; }
        
        public string CartName { get; set; }

        public string BTStatus { get; set; }

        public string CartFolderID { get; set; }

        public string CartFolderName { get; set; }

        public string OrgId { get; set; }

        public bool IsArchived { get; set; }

        public int LineItemCount { get; set; }

        public int TotalQuantity { get; set; }

        public int TotalOrderQuantity { get; set; }

        public int TotalCancelQuantity { get; set; }

        public int TotalBackOrderQuantity { get; set; }

        public int TotalInProcessQuantity { get; set; }

        public Decimal CartTotalListPrice { get; set; }

        public Decimal CartTotalNetPrice { get; set; }

        public Decimal ShippingTotal { get; set; }

        public Decimal FeeTotal { get; set; }

        public Decimal Total { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public string Note { get; set; }

        public string SpecialInstruction { get; set; }

        public List<CartAccount> CartAccounts { get; set; }

        public DateTime? NoteUpdatedDateTime { get; set; }

        public string NoteUpdatedBy { get; set; }

        public DateTime? SpecialInstructionsUpdatedDateTime { get; set; }

        public string SpecialInstructionsUpdatedBy { get; set; }

        public int TotalOELines { get; set; }

        public int TotalOEQuantity { get; set; }

        public int TitlesWithoutGrids { get; set; }

        public bool IsShared
        {
            get;
            set;
        }

        public bool HasProfile
        {
            get;
            set;
        }

        public bool HasGridLine { get; set; }

        public bool IsPremium
        {
            get;
            set;
        }

        /// <summary>
        /// Order form used for submitting order
        /// </summary>
        public OrderForm OrderForm { get; set; }

        public string CartOwnerId { get; set; }

        //user shared id, maybe cartownerid or pointer user id
        public string CartUserSharedId { get; set; }

        public string CartOwnerName { get; set; }

        public bool OneClickMARCIndicator { get; set; }

        public string FTPErrorMessage { get; set; }

        public int ESPStateTypeId { get; set; }

        public int ESPFundStateTypeID { get; set; }

        public int ESPDistStateTypeID { get; set; }

        public int ESPRankStateTypeId { get; set; }

        public string LastESPStateTypeLiteral { get; set; }

        public string ESPStateTypeLiteral { get; set; }
        public DateTime? SubmittedDate { get; set; }

        public bool HasESPRanking { get; set; }

        public int QuoteID { get; set; }

        public DateTime? QuoteExpiredDateTime { get; set; }

        public int BasketProcessingCategoryID { get; set; }

        public int FreezeLevel { get; set; }

        public bool IsActive { get; set; } // if TRUE, cart will be displayed in the Add/Copy/Move drop down (TFS 19189)
        public bool HasPermission { get; set; }
        public bool HasOwner { get; set; }
        public bool HasContribution { get; set; }

        public bool HasReview { get; set; }

        public bool HasAcquisition { get; set; }
        public bool HasWorkflow { get; set; }
        public bool HasReviewAcquisitionPermission { get; set; }
        public bool IsSplitting { get; set; }
        public bool IsSharedBasketGridEnabled { get; set; }

        public bool IsMixedProduct { get; set; }
        public bool IsTransferred { get; set; }
        public int GridDistributionOption { get; set; }
        /// <summary>
        /// 1: Contribution
        /// 2: Requisition
        /// 3: Review
        /// 4: Acquisition
        /// 0: if the basket doesn’t has workflow
        /// </summary>
        public int CurrentWorkflow { get; set; }
        public bool IsPrimary { get; set; }
        public bool ContainsAMixOfGridNNonGrid { get; set; }

        public string OrderedDownloadedUser { get; set; }
        #endregion
        #region Logging
        public string GetCartLogString()
        {
            var sb = new StringBuilder();

            sb.Append("CartID:" + CartId + "\"");
            sb.Append("UserId:" + UserId + "\"");
            sb.Append("CartName:" + CartName + "\"");
            sb.Append("BTStatus:" + BTStatus + "\"");
            sb.Append("CartFolderID:" + CartFolderID + "\"");
            sb.Append("CartFolderName:" + CartFolderName + "\"");
            sb.Append("IsArchived:" + IsArchived + "\"");
            sb.Append("LineItemCount:" + LineItemCount + "\"");
            sb.Append("TotalQuantity:" + TotalQuantity + "\"");
            sb.Append("CartTotalListPrice:" + CartTotalListPrice + "\"");
            sb.Append("CartTotalNetPrice:" + CartTotalNetPrice + "\"");
            sb.Append("CreatedBy:" + CreatedBy + "\"");
            sb.Append("UpdatedBy:" + UpdatedBy + "\"");
            sb.Append("CreatedDateTime:" + CreatedDateTime + "\"");
            sb.Append("Note:" + Note + "\"");
            sb.Append("SpecialInstruction:" + SpecialInstruction + "\"");

            return sb.ToString();
        }
        #endregion
    }

    public class HideEspAutoRankMessageRequest
    {
        public string CartId { get; set; }
        public string UserId { get; set; }
        public bool IsAutoRank { get; set; }
    }
}
