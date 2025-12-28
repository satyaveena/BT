using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Cache;
using BT.TS360API.Common.Configrations;
using BT.TS360API.Common.Helpers;
using BT.TS360Constants;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;

namespace BT.TS360API.Common.Pricing
{
    public class PricingConfiguration
    {
        private string _orderDbConnString;
        private string _productCatalogDbConnString;
        private string _exceptionLoggingDbConnString;
        private string _promotionServiceUrl;
        private string _tolasServiceUrl;
        private string[] _realTimeWsInfo;
        private const string RealTimeSysInfo = "__realTimeSysInfo";
        private string _galeLiteral;
        private string _sqlCmdTimeout;
        private string _promotionServiceBatchSize;
        private static PricingConfiguration _instance = new PricingConfiguration();

        public static PricingConfiguration Instance
        {
            get { return _instance ?? (_instance = new PricingConfiguration()); }
        }

        public string OrderDbConnString
        {
            set { _orderDbConnString = value; }
            get
            {
                if (string.IsNullOrEmpty(_orderDbConnString))
                {
                    _productCatalogDbConnString = AppSettings.OrderDbConnString;
                    //if (SPContext.Current != null)
                    //{
                    //    _orderDbConnString =
                    //        GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.OrdersConnectionstring).Value;
                    //}
                    //else
                    //{
                    //    _orderDbConnString = System.Configuration.ConfigurationManager.AppSettings[GlobalConfigurationKey.OrdersConnectionstring];
                    //}
                }
                return _orderDbConnString;
            }
        }

        public string ProductCatalogDbConnString
        {
            set { _productCatalogDbConnString = value; }
            get
            {
                if (string.IsNullOrEmpty(_productCatalogDbConnString))
                {
                    _productCatalogDbConnString = AppSettings.ProductCatalogConnectionString;
                    //if (SPContext.Current != null)
                    //{
                    //    _productCatalogDbConnString =
                    //        GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ProductcatalogConnectionstring).
                    //            Value;
                    //}
                    //else
                    //{
                    //    _productCatalogDbConnString =
                    //        System.Configuration.ConfigurationManager.AppSettings[
                    //            GlobalConfigurationKey.ProductcatalogConnectionstring];
                    //}
                }
                return _productCatalogDbConnString;
            }
        }

        public string ExceptionLoggingDbConnString
        {
            set { _exceptionLoggingDbConnString = value; }
            get
            {
                if (string.IsNullOrEmpty(_exceptionLoggingDbConnString))
                {
                    _exceptionLoggingDbConnString = AppSettings.LogsConnectionstring;
                    //if (SPContext.Current != null)
                    //{
                    //    _exceptionLoggingDbConnString =
                    //        GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.LogsConnectionstring).
                    //            Value;
                    //}
                    //else
                    //{
                    //    _exceptionLoggingDbConnString =
                    //        System.Configuration.ConfigurationManager.AppSettings[
                    //            GlobalConfigurationKey.LogsConnectionstring];
                    //}
                }
                return _exceptionLoggingDbConnString;
            }
        }

        public string PromotionServiceUrl
        {
            set { _promotionServiceUrl = value; }
            get
            {
                if (string.IsNullOrEmpty(_promotionServiceUrl))
                {
                    _promotionServiceUrl = AppSettings.TolasPricingServiceUrl;
                    //if (SPContext.Current != null)
                    //{
                    //    _promotionServiceUrl =
                    //        GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.PromotionServiceUrl).Value;
                    //}
                    //else
                    //{
                    //    _promotionServiceUrl =
                    //        System.Configuration.ConfigurationManager.AppSettings[
                    //            GlobalConfigurationKey.PromotionServiceUrl];
                    //}

                }
                return _promotionServiceUrl;
            }
        }

        public string TolasServiceUrl
        {
            set { _tolasServiceUrl = value; }
            get
            {
                if (string.IsNullOrEmpty(_tolasServiceUrl))
                {
                    _tolasServiceUrl = AppSettings.TolasPricingServiceUrl;
                    //if (SPContext.Current != null)
                    //{
                    //    _tolasServiceUrl =
                    //        GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.TolasPricingServiceUrl).Value;
                    //}
                    //else
                    //{
                    //    _tolasServiceUrl =
                    //        System.Configuration.ConfigurationManager.AppSettings[
                    //            GlobalConfigurationKey.TolasPricingServiceUrl];
                    //}
                }
                return _tolasServiceUrl;
            }
        }

        public string[] RealTimeWsInfo
        {
            set { _realTimeWsInfo = value; }
            get
            {
                if (_realTimeWsInfo == null || _realTimeWsInfo.Length < 2)
                {
                    string sysId = AppSettings.RealtimeWsSysid;// GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.RealtimeWsSysid).Value;
                    string sysPass = AppSettings.RealtimeWsSyspass;// GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.RealtimeWsSyspass).Value;
                    if (!String.IsNullOrEmpty(sysId) && !String.IsNullOrEmpty(sysPass))
                    {
                        _realTimeWsInfo = new string[2];
                        _realTimeWsInfo[0] = sysId;
                        _realTimeWsInfo[1] = sysPass;
                    }

                    //if (SPContext.Current != null)
                    //{
                    //    _realTimeWsInfo = GetRealTimeWSInfo();
                    //}
                    //else
                    //{
                    //    string sysId = System.Configuration.ConfigurationManager.AppSettings[GlobalConfigurationKey.RealtimeWsSysid];
                    //    string sysPass = System.Configuration.ConfigurationManager.AppSettings[GlobalConfigurationKey.RealtimeWsSyspass];
                    //    if (!String.IsNullOrEmpty(sysId) && !String.IsNullOrEmpty(sysPass))
                    //    {
                    //        _realTimeWsInfo = new string[2];
                    //        _realTimeWsInfo[0] = sysId;
                    //        _realTimeWsInfo[1] = sysPass;
                    //    }
                    //}
                }
                return _realTimeWsInfo;
            }
        }

        private string[] GetRealTimeWSInfo()
        {
            string[] realTimeWSInfo = null;
            var sysInfo = CachingController.Instance.Read(RealTimeSysInfo);
            if (sysInfo == null)
            {
                try
                {
                    string sysId = AppSettings.RealtimeWsSysid;// GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.RealtimeWsSysid).Value;
                    string sysPass = AppSettings.RealtimeWsSyspass;// GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.RealtimeWsSyspass).Value;
                    if (!String.IsNullOrEmpty(sysId) && !String.IsNullOrEmpty(sysPass))
                    {
                        realTimeWSInfo = new string[2];
                        realTimeWSInfo[0] = sysId;
                        realTimeWSInfo[1] = sysPass;
                        CachingController.Instance.Write(RealTimeSysInfo, realTimeWSInfo);
                    }
                }
                catch (Exception)
                {
                    realTimeWSInfo = null;
                }
            }
            else
            {
                realTimeWSInfo = sysInfo as string[];
            }

            return realTimeWSInfo;
        }

        public string GaleLiteral
        {
            set { _galeLiteral = value; }
            get
            {
                if (string.IsNullOrEmpty(_galeLiteral))
                {
                    _galeLiteral = AppSettings.GaleLiteral;// System.Configuration.ConfigurationManager.AppSettings[GlobalConfigurationKey.GaleLiteral];

                    //if (SPContext.Current != null)
                    //{
                    //    _galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
                    //}
                    //else
                    //{
                    //    _galeLiteral = System.Configuration.ConfigurationManager.AppSettings[GlobalConfigurationKey.GaleLiteral];
                    //}
                }
                return _galeLiteral;
            }
        }

        public string SqlCmdTimeout
        {
            set { _sqlCmdTimeout = value; }
            get
            {
                if (string.IsNullOrEmpty(_sqlCmdTimeout))
                {
                    _sqlCmdTimeout = AppSettings.PricingSqlCmdTimeout;// System.Configuration.ConfigurationManager.AppSettings[GlobalConfigurationKey.SqlCmdTimeout];
                    //if (SPContext.Current != null)
                    //{
                    //    _sqlCmdTimeout = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.SqlCmdTimeout).Value;
                    //}
                    //else
                    //{
                    //    _sqlCmdTimeout = System.Configuration.ConfigurationManager.AppSettings[GlobalConfigurationKey.SqlCmdTimeout];
                    //}
                }
                return _sqlCmdTimeout;
            }
        }

        public string PromotionServiceBatchSize
        {
            set { _promotionServiceBatchSize = value; }
            get
            {
                if (string.IsNullOrEmpty(_promotionServiceBatchSize))
                {
                    _promotionServiceBatchSize = AppSettings.PromotionServiceBatchSize;
                    //if (SPContext.Current != null)
                    //{
                    //    _promotionServiceBatchSize = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.PromotionServiceBatchSize).Value;
                    //}
                    //else
                    //{
                    //    _promotionServiceBatchSize = System.Configuration.ConfigurationManager.AppSettings[GlobalConfigurationKey.PromotionServiceBatchSize];
                    //}
                }
                return _promotionServiceBatchSize;
            }
        }
    }
}
