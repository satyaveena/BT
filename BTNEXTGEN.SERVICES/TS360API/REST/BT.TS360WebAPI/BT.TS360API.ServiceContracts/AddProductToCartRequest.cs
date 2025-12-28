using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class AddProductToCartNameRequest : BaseRequest
    {
        public List<AddToNewCartObject> addToNewCartObjects { get; set; }
        public string cartName { get; set; }
        public int? defaultGridQuantity { get; set; }

        public string UserId { get; set; }
        public string DefaultQuantity { get; set; }
        public TargetingValues Targeting { get; set; }
    }

    public class AddProductWithGridToPrimaryCartRequest : BaseRequest
    {
        public List<AddToNewCartObject> AddToNewCartObjects { get; set; }
        public List<DCGridTitleProperty> GridTitleProperties { get; set; }

        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string DefaultQuantity { get; set; }
        public TargetingValues Targeting { get; set; }
    }

    public class AddProductWithGridToSelectedCartRequest : BaseRequest
    {
        public List<AddToNewCartObject> AddToNewCartObjects { get; set; }
        public List<DCGridTitleProperty> GridTitleProperties { get; set; }
        public string CartName { get; set; }
        public string CartId { get; set; }
        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string DefaultQuantity { get; set; }
        public TargetingValues Targeting { get; set; }
    }

    public class SimpleProductToCartRequest
    {
        public string BTKey { get; set; }
        public int Quantity { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }

        public string CartId { get; set; }
        public string UserId { get; set; }
       // public string CartName { get; set; }
        //public string OrgId { get; set; }
    }
}
