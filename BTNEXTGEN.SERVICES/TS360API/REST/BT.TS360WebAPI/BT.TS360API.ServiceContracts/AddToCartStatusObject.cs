using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{    
    public class AddToCartStatusObject
    {
        public bool IsPrimary { get; set; }
         
        public string UserID { get; set; }
         
        public string OrgID { get; set; }
         
        public List<CartInfo> CartInfo { get; set; }
         
        public int LineCountSuccess { get; set; }
         
        public int ItemCountSuccess { get; set; }
         
        public int LineCountFail { get; set; }
         
        public int ItemCountFail { get; set; }
    }
    public class CartInfo
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string URL { get; set; }
        public string LineItemID { get; set; }
    }
}
