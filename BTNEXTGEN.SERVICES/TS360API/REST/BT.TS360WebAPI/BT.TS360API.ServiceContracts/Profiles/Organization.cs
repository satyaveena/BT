using BT.TS360Constants;
using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts.Profiles
{
    public class Organization
    {
        public string DefaultVIPAccountId { get; set; }
        public string OrgId { get; set; }
        public string[] ReviewTypeList { get; set; }
        public string DefaultBookAccountId { get; set; }
        public string DefaultEntAccountId { get; set; }
        public string DefaultOneBoxAccountId { get; set; }

        public List<Account> Accounts { get; set; }

        public List<string> DefaulteSuppliersAccountList { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string BTOrganizationId { get; set; }
        public string Status { get; set; }
        public string WebMarketTypeName { get; set; }
        public int AccountCount { get; set; }
        public int UsersUseCount { get; set; }
        public int AllActiveUsersCount { get; set; }
        public string AdminContact { get; set; }
        public int AllowedUsersCount { get; set; }
        public string WebMarketType { get; set; }
        public int AllUsersCount { get; set; }
        public string OrganizationType { get; set; }
        public string OrganizationTypeName { get; set; }
        public bool IsBTEmployee { get; set; }
        public string ContactName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string SalesRep { get; set; }
        public int BTUsersCount { get; set; }
        public bool? AllWarehouse { get; set; }

        public string PersonalProductURL { get; set; }

        public string ProductLookupIndex { get; set; }

        public string ProductSuffixLookup { get; set; }

        public string ISBNLookupCode { get; set; }

        public string ISBNLinkDisplayed { get; set; }

        public bool? EntertainmentProduct { get; set; }

        public bool? TableOfContents { get; set; }

        public string AVPersonalProductURL { get; set; }

        public string AVProductLookupIndex { get; set; }

        public string AVProductSuffixLookup { get; set; }

        public bool? AVUseUPC14 { get; set; }

        public bool? AVUseISBN { get; set; }

        public bool? ProductLookupDeactivated { get; set; }

        public bool? AVProductLookupDeactivated { get; set; }

        public bool? IsAuthorizedtoUseAllGridCodes { get; set; }
  
        public List<UserProfile> Users { get; set; }
        public bool? IsOCLCCatalogingPlusEnabled { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactFax { get; set; }
        public bool? GlobalAffidavit { get; set; }
        public bool? WebOrder { get; set; }
        public bool? IsFullMarcProfile { get; set; }
        public string[] ESuppliers { get; set; }
        public bool? LibrarySystemHandling { get; set; }
        public bool? ShowBibNumber { get; set; }
        public bool? OriginalEntry { get; set; }
        public string TimeZone { get; set; }
        public decimal? BookProcessingCharge { get; set; }
        public decimal? PaperbackProcessingCharge { get; set; }
        public decimal? SpokenWordProcessingCharge { get; set; }
        public decimal? MovieProcessingCharge { get; set; }
        public decimal? MusicProcessingCharge { get; set; }
        public float? SalesTax { get; set; }
        public AssociationType AssociationType { get; set; }
        public bool? SlipReport { get; set; }
        public bool IsGridEnable { get; set; }
        public bool? MARCProfilerEnabled { get; set; }
        public bool? OCLCCatalogingPlusEnabled { get; set; }

        public string ILSAcquisitionsEnabled { get; set; }
        public string ILSAcquisitionsApiURL { get; set; }
        public string ILSAcquisitionsApiKey { get; set; }
        public string ILSAcquisitionsApiPassphrase { get; set; }
        public string ILSAcquisitionsUserId { get; set; }
    }
}
