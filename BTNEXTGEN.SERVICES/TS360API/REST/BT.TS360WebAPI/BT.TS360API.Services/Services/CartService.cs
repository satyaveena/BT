using BT.TS360API.Common.Business;
using BT.TS360API.Common.Constants;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helper;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using BT.TS360SP;
using System;
using System.Collections.Generic;

namespace BT.TS360API.Services.Services
{
    public class CartService
    {
        #region HideEspAutoRankMessage
        public bool HideEspAutoRankMessage(HideEspAutoRankMessageRequest request)
        {
            return CartDAOManager.HideEspAutoRankMessage(request);
        }
        #endregion
    }
}