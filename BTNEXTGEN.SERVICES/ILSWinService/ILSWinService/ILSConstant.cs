using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSWinService
{
    public class ILSConstant
    {
        public const string ILSBaseAddress = "ILSBaseAddress";
        public const string ILSOrderValidatePath = "ILSOrderValidatePath";
        public const string ILSSubmitOrderPath = "ILSSubmitOrderPath";
        public const string ILSAuthorizePath = "ILSAuthorizePath";
        public const string ILSTokenPath = "ILSTokenPath";
        public const string ILSGetLogApiUrl = "ILSGetLogApiUrl";
        public const string ILSInsertLogApiUrl = "ILSInsertLogApiUrl";
        public const string ILSVendor = "ILSVendor";

        public const string Proc_Get_ILS_Pending_Order_Count = "procTS360GetILSPendingOrderCount";
        public const string Proc_Get_ILS_Pending_Order = "procTS360GetILSPendingOrder";
        public const string Proc_Set_ILS_Basket_Status = "procTS360SetILSBasketState";
    }
}
