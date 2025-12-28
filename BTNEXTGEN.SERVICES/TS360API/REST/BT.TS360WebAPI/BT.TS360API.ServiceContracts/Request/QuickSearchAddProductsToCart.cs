
using System.Collections.Generic;


namespace BT.TS360API.ServiceContracts.Request
{
    public class QuickSearchAddProductsToCart : BaseRequest
    {
        public List<AddToNewCartObject> addToNewCartObjects { get; set; }
        public string desFolderId { get; set; }
        public string cartName { get; set; }
        public string cartId { get; set; }

        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string DefaultQuantity { get; set; }
        public bool IsGridEnabled { get; set; }
        public bool IsAuthorizedtoUseAllGridCodes { get; set; }
        public TargetingValues Targeting { get; set; }
    }
}
