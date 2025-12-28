using System;
using System.Collections.Generic;
using BT.TS360API.Common.Search;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360Constants;

namespace BT.TS360API.Common.Pricing
{
    public class RealTimePricingHelper : IDisposable
    {
        private Dictionary<string, AccountInfo4Pricing> _accountInfo4Pricings = new Dictionary<string, AccountInfo4Pricing>();

        public AccountInfo4Pricing GetAccountInfoForPricing(string productType, string eSupplierDisplayText, string productFormat,
            string userId, AccountInfoForPricing accountPricingData)
        {
            try
            {
                //building key
                var normalKey = productType + eSupplierDisplayText;
                var withFormat = string.Empty;
                if (accountPricingData.EnableProcessingCharges&& String.Compare(productType, ProductTypeConstants.Book, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (string.Compare(productFormat, ProductFormatConstants.Book_Paperback, StringComparison.OrdinalIgnoreCase) == 0
                        || string.Compare(productFormat, ProductFormatConstants.Book_CompactDisc, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        withFormat = productFormat;
                    }
                }

                var key = normalKey + withFormat;
                //find key in local cache
                if (_accountInfo4Pricings.ContainsKey(key))
                    return _accountInfo4Pricings[key];

                //get account info
                var requiredInfor = ProductSearchController.GetAccountInfoForPricing(productType, eSupplierDisplayText, userId, accountPricingData);

                var tempCharges = requiredInfor.ProcessingCharges;//book or movie
                if (String.Compare(productType, ProductTypeConstants.Music, StringComparison.OrdinalIgnoreCase) == 0)
                    tempCharges = requiredInfor.ProcessingCharges2;

                if (!_accountInfo4Pricings.ContainsKey(normalKey))
                {
                    _accountInfo4Pricings.Add(normalKey, new AccountInfo4Pricing()
                    {
                        AccountId = requiredInfor.AccountId,
                        PrimaryWarehouseCode = requiredInfor.PrimaryWarehouseCode,
                        AccountPricePlanId = requiredInfor.AccountPricePlanId,
                        ErpAccountNumber = requiredInfor.ErpAccountNumber,
                        EMarketType = requiredInfor.EMarketType,
                        ETier = requiredInfor.ETier,
                        IsHomeDelivery = requiredInfor.IsHomeDelivery,
                        BuildingNumbers = requiredInfor.BuildingNumbers,
                        ProcessingCharges = tempCharges,
                        SalesTax = requiredInfor.SalesTax,
                        IsVIPAccount = requiredInfor.IsVIPAccount
                    });
                }

                if (string.IsNullOrEmpty(withFormat))
                    return _accountInfo4Pricings.ContainsKey(normalKey) ? _accountInfo4Pricings[normalKey] : new AccountInfo4Pricing();

                var tempkey = normalKey + ProductFormatConstants.Book_Paperback;
                if (!_accountInfo4Pricings.ContainsKey(tempkey))
                {
                    _accountInfo4Pricings.Add(tempkey, new AccountInfo4Pricing()
                    {
                        AccountId = requiredInfor.AccountId,
                        PrimaryWarehouseCode = requiredInfor.PrimaryWarehouseCode,
                        AccountPricePlanId = requiredInfor.AccountPricePlanId,
                        ErpAccountNumber = requiredInfor.ErpAccountNumber,
                        EMarketType = requiredInfor.EMarketType,
                        ETier = requiredInfor.ETier,
                        IsHomeDelivery = requiredInfor.IsHomeDelivery,
                        BuildingNumbers = requiredInfor.BuildingNumbers,
                        ProcessingCharges = requiredInfor.ProcessingCharges + requiredInfor.ProcessingCharges2,
                        SalesTax = requiredInfor.SalesTax,
                        IsVIPAccount = requiredInfor.IsVIPAccount
                    });
                }

                tempkey = normalKey + ProductFormatConstants.Book_CompactDisc;
                if (!_accountInfo4Pricings.ContainsKey(tempkey))
                {
                    _accountInfo4Pricings.Add(tempkey, new AccountInfo4Pricing()
                    {
                        AccountId = requiredInfor.AccountId,
                        PrimaryWarehouseCode = requiredInfor.PrimaryWarehouseCode,
                        AccountPricePlanId = requiredInfor.AccountPricePlanId,
                        ErpAccountNumber = requiredInfor.ErpAccountNumber,
                        EMarketType = requiredInfor.EMarketType,
                        ETier = requiredInfor.ETier,
                        IsHomeDelivery = requiredInfor.IsHomeDelivery,
                        BuildingNumbers = requiredInfor.BuildingNumbers,
                        ProcessingCharges = requiredInfor.ProcessingCharges + requiredInfor.SpokenWordCharge,
                        SalesTax = requiredInfor.SalesTax,
                        IsVIPAccount = requiredInfor.IsVIPAccount
                    });
                }
                return _accountInfo4Pricings.ContainsKey(key) ? _accountInfo4Pricings[key] : new AccountInfo4Pricing();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new AccountInfo4Pricing();
            }
        }

        public void Dispose()
        {
            if (_accountInfo4Pricings != null)
            {
                _accountInfo4Pricings.Clear();
                _accountInfo4Pricings = null;
            }
        }
    }
}
