using BT.TS360Constants;
using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts.Profiles
{
    public class UserProfile
    {
        public string PIGName { get; set; }
        public string DefaultVIPAccountId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string UserName { get; set; }

        //public Organization OrganizationEntity { get; set; }
        public string DefaultBookAccountId { get; set; }
        public string DefaultEntAccountId { get; set; }
        public string DefaultOneBoxAccountId { get; set; }
        public int? DefaultQuantity { get; set; }
        public string DefaultDuplicateOrders { get; set; }
        public string DefaultDuplicateCarts { get; set; }
        public string OrgId { get; set; }
        public string HoldingsFlag { get; set; }
        public string DefaultDownloadedCarts { get; set; }

        public List<Account> AccountCreateCarts { get; set; }
        public List<Account> AccountViewOrders { get; set; }

        public List<string> DefaultESupplierAccountsList { get; set; }
        public List<UserReviewType> MyReviewTypes { get; set; }
        public List<string> MyReviewTypeIds { get; set; }
        
        //TODO:
        
        public string[] ProductTypeFilter { get; set; }
        public string LastName { get; set; }
        public string UserAlias { get; set; }
        public Organization Organization { get; set; }
        public string OrgName { get; set; }
        public string AllUsersCount { get; set; }
        public string[] ProductTypeList { get; set; }
        public List<string> FunctionList { get; set; }
        public bool? IsBTEmployee { get; set; }
        public AssociationType AssociationType { get; set; }
        public string SalesRepName { get; set; }
        public string Email { get; set; }
        public string[] AudienceTypeList { get; set; }
        public int AccountCount { get; set; }
        public string BTAccountStatus { get; set; }
        public string DefaultMARCProfileId { get; set; }
        public string CartSortOrder { get; set; }
        public string CartSortBy { get; set; }

        public string BookIncludeFilter { get; set; }
        public string BookExcludeFilter { get; set; }
        public string MusicIncludeFilter { get; set; }
        public string MusicExcludeFilter { get; set; }
        public string MovieIncludeFilter { get; set; }
        public string MovieExcludeFilter { get; set; }
        public string DigitalIncludeFilter { get; set; }
        public string DigitalExcludeFilter { get; set; }

        public string AltFormatSearchSortBy { get; set; }
        public string AltFormatSearchSortOrder { get; set; }
        public string AltFormatCartSortOrder { get; set; }
        public string AltFormatCartSortBy { get; set; }
    }
}
