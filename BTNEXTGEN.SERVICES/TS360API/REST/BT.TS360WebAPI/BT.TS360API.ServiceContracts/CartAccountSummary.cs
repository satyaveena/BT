//-----------------------------------------------------------------------
// <copyright file="ItemData.cs" company="Microsoft">
//     Copyright © Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary> Item Data </summary>
//-----------------------------------------------------------------------

namespace BT.TS360API.ServiceContracts
{
    /// <summary>
    /// The item data class
    /// </summary>
    ///     
    public class CartAccountSummary
    {
        public string AccountID;
        public string BasketAccountTypeID;
        public string PONumber;
        public string AccountAlias;
        public CartAccountSummary()
        {
        }
    }
}