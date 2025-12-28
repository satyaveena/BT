using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BT.TS360API.Common.Helpers
{
    public class ContentCafeConfiguration
    {
        public ContentCafeConfiguration()
        {
        }

        public string AccessKey
        {
            get
            {
                var systemId = string.Empty;
                var settingKey = GlobalConfigurationKey.CcCloudImageNextgenSystemid;
                var setting = ConfigurationManager.AppSettings[settingKey];
                if (setting != null)
                {
                    systemId = setting;
                }
                return systemId;
            }
        }

        public string ImageUrlFormat
        {
            get
            {
                string contentCafeImageUrl = CommonConstants.DefaultImageUrl;

                var imageConnectionString = GlobalConfigurationKey.CcCloudImageConnectionstring;
                var setting = ConfigurationManager.AppSettings[imageConnectionString];
                if (setting != null)
                {
                    contentCafeImageUrl = setting;
                }

                return contentCafeImageUrl;
            }
        }

        public string GetImageSize(ImageSize imageSize)
        {
            string jacketSize;
            string mediumSettingKey;
            string largeSettingKey;
            string smallSettingKey;

            mediumSettingKey = GlobalConfigurationKey.CcCloudImage_Size_Medium;
            largeSettingKey = GlobalConfigurationKey.CcCloudImage_Size_Large;
            smallSettingKey = GlobalConfigurationKey.CcCloudImage_Size_Small;
            
            switch (imageSize)
            {
                case ImageSize.Medium:
                    jacketSize = ConfigurationManager.AppSettings[mediumSettingKey];
                    break;
                case ImageSize.Large:
                    jacketSize = ConfigurationManager.AppSettings[largeSettingKey];
                    break;
                default:
                    jacketSize = ConfigurationManager.AppSettings[smallSettingKey];
                    break;
            }

            return jacketSize;
        }
    }
}
