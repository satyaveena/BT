using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class Basket
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TotalBackOrderQuantity { get; set; }
        public int TotalCancelQuantity { get; set; }
        public decimal Total { get; set; }
        public decimal CartTotalListPrice { get; set; }
        public decimal CartTotalNetPrice { get; set; }
        public decimal CartTotal { get; set; }
        public string OrganizationId { get; set; }
        public string UserId { get; set; }
        public string CartUserSharedId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DateCreated { get; set; }
        public string SpecialInstructions { get; set; }
        public string BTNote { get; set; }
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public int LineItemsCount { get; set; }
        public string BTStatus { get; set; }
        public string BookAccountId { get; set; }
        public string EntertainmentAccountId { get; set; }
        public int IsArchived { get; set; }
        public int ItemsCount { get; set; }
        public decimal ShippingTotal { get; set; }
        public string CartOwner { get; set; }
        public bool HasProfile { get; set; }
        public bool IsShared { get; set; }
        public bool HasGridLine { get; set; }
        public bool IsPremium { get; set; }
        public bool HasPermission { get; set; }
        public bool OneClickMARCIndicator { get; set; }
        public string FTPErrorMessage { get; set; }
        public bool HasOwner { get; set; }
        public bool HasContribution { get; set; }
        public bool HasReview { get; set; }
        public bool HasAcquisition { get; set; }
        public int CurrentWorkflowStage { get; set; }
        public bool HasWorkflow { get; set; }
        public bool HasReviewAcquisitionPermission { get; set; }
        public bool IsMixedProduct { get; set; }
        public int ESPStateTypeId { get; set; }
        public bool HasESPRanking { get; set; }
        public string ESPStateTypeLiteral { get; set; }
        public bool ContainsAMixOfGridNNonGrid { get; set; }
        public int IsPrimary { get; set; }
        public string BookAccountName { get; set; }
        public string BookAccountERPNumber { get; set; }
        public string EntAccountName { get; set; }
        public string EntAccountERPNumber { get; set; }
        public static string OPEN = "Open";
        public static int MaximumBasketNameLength = 80;
        public static string DELETED = "Deleted";
        public static string SUBMITTED = "Submitted";
        public static string DOWNLOAED = "Downloaded";
        public static string ORDERED = "Ordered";
        public static string QUOTED = "Quoted";
        public static string QUOTESUBMITTED = "Quote Submitted";
        public static string QUOTETRANSMITTED = "Quote Transmitted";
        public static string VIPSUBMITTED = "VIP Submitted";
        public static string VIPORDERED = "VIP Ordered";
        public static string ORDEREDQUOTE = "Ordered Quote";
        public static string ILSSUBMITTED = "ILS Submitted";
        public static string ILSORDERED = "ILS Ordered";

    }
}
