using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using BT.TS360API.Cache;
using BT.TS360API.ExternalServices.Controller;
using BT.TS360API.ExternalServices.CSProfileServiceReference;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;

namespace BT.TS360API.ExternalServices
{
    public class ProfileService
    {
        private int DefaultProfileCacheDuration = 30;
        private static ProfilesWebServiceSoapClient _serviceClient;

        private static volatile ProfileService _instance;
        private static readonly object SyncRoot = new Object();

        private ProfileService()
        {
            var temp = ConfigurationManager.AppSettings["CachingDurationForProfileInfo"];
            if (string.IsNullOrEmpty(temp))
            {
                DefaultProfileCacheDuration = 30;
            }
            else
            {
                int tempInt;
                if (!int.TryParse(temp, out tempInt))
                {
                    tempInt = 30;
                }
                DefaultProfileCacheDuration = tempInt;
            }
        }

        public static ProfileService Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _serviceClient = new ProfilesWebServiceSoapClient(); 
                        _instance = new ProfileService();
                    }
                        
                }

                return _instance;
            }
        }

        public UserProfile GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            var cacheKey = string.Format(DistributedCacheKey.ProfileServiceUserCacheKey, userId);
            var userProfile = CachingController.Instance.Read(cacheKey) as UserProfile;

            if (userProfile != null) return userProfile;

            var userObjectXml = _serviceClient.GetProfile(userId, ProfileEntity.UserObject.ToString());

            if (userObjectXml == null) return null;

            userProfile = new UserProfile();
            userProfile.UserId = userId;

            GetValuesForAccountInfo(userObjectXml, userProfile);

            GetValuesForBtNextGen(userObjectXml, userProfile);

            GetValuesForMyPref(userObjectXml, userProfile);

            CachingController.Instance.Write(cacheKey, userProfile, DefaultProfileCacheDuration);

            return userProfile;
        }

        public List<UserReviewType> GetUserReviewTypes(string userId, List<string> ids)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            var reviewTypes = CSProfileController.Instance.GetReviewTypes(ids);
            return reviewTypes;
        }

        private static void ExpireUserObjectCache(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return;

            var cacheKey = string.Format(DistributedCacheKey.ProfileServiceUserCacheKey, userId);
            CachingController.Instance.SetExpired(cacheKey);
        }

        public void ToggleQuickSearchView(string userId, bool isQuickSearchEnabled)
        {
            if (string.IsNullOrEmpty(userId)) return;

            var userObjectXml = _serviceClient.GetProfile(userId, ProfileEntity.UserObject.ToString());
            var selectSingleNode = userObjectXml.SelectSingleNode("//UserObject/MyPreferences/is_quick_search_view_enabled");
            if (selectSingleNode != null)
                selectSingleNode.InnerText = isQuickSearchEnabled ? "1" : "0";

            _serviceClient.UpdateProfile(ref userObjectXml, true, false);
        }

        public void ToggleQuickItemDetails(string userId, bool isQuickItemDetailsEnabled)
        {
            if (string.IsNullOrEmpty(userId)) return;

            var userObjectXml = _serviceClient.GetProfile(userId, ProfileEntity.UserObject.ToString());
            var selectSingleNode = userObjectXml.SelectSingleNode("//UserObject/MyPreferences/is_quick_item_details_enabled");
            if (selectSingleNode != null)
                selectSingleNode.InnerText = isQuickItemDetailsEnabled ? "1" : "0";

            _serviceClient.UpdateProfile(ref userObjectXml, true, false);
        }

        public void ToggleQuickCartDetails(string userId, bool isQuickCartDetailsEnabled)
        {
            if (string.IsNullOrEmpty(userId)) return;

            var userObjectXml = _serviceClient.GetProfile(userId, ProfileEntity.UserObject.ToString());
            var selectSingleNode = userObjectXml.SelectSingleNode("//UserObject/MyPreferences/is_quick_cart_details_enabled");
            if (selectSingleNode != null)
                selectSingleNode.InnerText = isQuickCartDetailsEnabled ? "1" : "0";

            _serviceClient.UpdateProfile(ref userObjectXml, true, false);
        }

        public void ToggleQuickCartList(string userId, bool isQuickCartsListEnabled)
        {
            if (string.IsNullOrEmpty(userId)) return;

            var userObjectXml = _serviceClient.GetProfile(userId, ProfileEntity.UserObject.ToString());
            var selectSingleNode = userObjectXml.SelectSingleNode("//UserObject/MyPreferences/is_quick_carts_list_enabled");
            if (selectSingleNode != null)
                selectSingleNode.InnerText = isQuickCartsListEnabled ? "1" : "0";

            _serviceClient.UpdateProfile(ref userObjectXml, true, false);
        }

        private void ToggleProductImages(string userId, bool updatedValue, string dataKey)
        {
            if (string.IsNullOrEmpty(userId)) return;

            var userObjectXml = _serviceClient.GetProfile(userId, ProfileEntity.UserObject.ToString());
            var selectSingleNode = userObjectXml.SelectSingleNode("//UserObject/MyPreferences/" + dataKey);
            if (selectSingleNode != null)
                selectSingleNode.InnerText = updatedValue ? "1" : "0";

            _serviceClient.UpdateProfile(ref userObjectXml, true, false);
            ExpireUserObjectCache(userId);
        }

        public void ToggleProductImagesForQuickSearch(string userId, bool updatedValue)
        {
            if (string.IsNullOrEmpty(userId)) return;

            ToggleProductImages(userId, updatedValue, "is_hide_product_images");
        }
        public void ToggleProductImagesForQuickCart(string userId, bool updatedValue)
        {
            if (string.IsNullOrEmpty(userId)) return;

            ToggleProductImages(userId, updatedValue, "is_hide_product_images_for_quick_cart");
        }

        public Organization GetOrganizationById(string orgId)
        {
            if (string.IsNullOrEmpty(orgId)) return null;

            var cacheKey = string.Format(DistributedCacheKey.ProfileServiceOrgCacheKey, orgId);
            var orgProfile = CachingController.Instance.Read(cacheKey) as Organization;

            if (orgProfile != null) return orgProfile;

            var orgObjectXml = _serviceClient.GetProfile(orgId, ProfileEntity.Organization.ToString());

            if (orgObjectXml == null) return null;

            orgProfile = new Organization();
            orgProfile.OrgId = orgId;

            GetOrgValuesForGeneralInfo(orgObjectXml, orgProfile);

            CachingController.Instance.Write(cacheKey, orgProfile, DefaultProfileCacheDuration);

            return orgProfile;
        }

        public Account GetAccountById(string accountId)
        {
            if (string.IsNullOrEmpty(accountId)) return null;

            var objectXml = _serviceClient.GetProfile(accountId, ProfileEntity.BTAccount.ToString());

            if (objectXml == null) return null;

            var profile = new Account(accountId);

            GeAccountValuesForGeneralInfo(objectXml, profile);

            return profile;
        }

        public Warehouse GetWarehouseById(string warehouseId)
        {
            if (string.IsNullOrEmpty(warehouseId)) return null;

            var objectXml = _serviceClient.GetProfile(warehouseId, ProfileEntity.BTWarehouse.ToString());

            if (objectXml == null) return null;

            var warehouse = GeWarehouseValuesForGeneralInfo(objectXml);

            return warehouse;
        }

        #region private

        private Warehouse GeWarehouseValuesForGeneralInfo(XmlElement orgObjectXml)
        {
            Warehouse warehouse = new Warehouse();
            var xml = orgObjectXml.SelectNodes("/BTWarehouse/BTNextGen");
            if (xml != null)
            {
                foreach (XmlNode node in xml)
                {
                    if (node.Attributes == null) continue;

                    warehouse.Id = GetSingleNodeText(node, "warehouse_id");
                    warehouse.Code = GetSingleNodeText(node, "warehouse_erp_code");
                    warehouse.Description = GetSingleNodeText(node, "warehouse_description");
                }
            }

            return warehouse;
        }

        private void GeAccountValuesForGeneralInfo(XmlElement orgObjectXml, Account account)
        {
            var xml = orgObjectXml.SelectNodes("/BTAccount/BTNextGen");
            if (xml != null)
            {
                foreach (XmlNode node in xml)
                {
                    if (node.Attributes == null) continue;

                    SetAccountFromXmlNode(account, node);
                }
            }
        }

        private void SetAccountFromXmlNode(Account account, XmlNode node)
        {
            if (string.IsNullOrEmpty(account.AccountId))
            {
                account.AccountId = GetSingleNodeText(node, "account_id");
            }

            account.IsTOLAS = GetSingleNodeText(node, "is_TOLAS") == "1";
            account.Account8Id = GetSingleNodeText(node, "account8_id");
            account.ProductType = GetSingleNodeText(node, "product_type");
            account.AccountType = GetSingleNodeText(node, "account_type");
            account.AccountNumber = GetSingleNodeText(node, "erp_account_number");
            account.DisabledReasonCode = GetSingleNodeText(node, "disable_reason_code");
            account.PrimaryWarehouseCode = account.PrimaryWarehouseName = GetSingleNodeText(node, "primary_warehouse");
            account.SecondaryWarehouseCode = account.SecondaryWarehouseName = GetSingleNodeText(node, "secondary_warehouse");
            account.EMarketType = GetSingleNodeText(node, "esupplier_market_type");
            account.ETier = GetSingleNodeText(node, "esupplier_market_tier");
            account.SOPPricePlanId = GetSingleNodeText(node, "sop_price_plan_list");
            account.IsBillingAccount = GetSingleNodeText(node, "is_billing_account") == "1";
            account.HomeDeliveryAccount = GetSingleNodeText(node, "is_home_delivery") == "1";
            account.ESupplier = GetSingleNodeText(node, "eSupplier");
            account.CheckLEReserve = GetSingleNodeText(node, "check_le_reserve") == "1";
            account.AccountInventoryType = GetSingleNodeText(node, "inventory_type");
            account.InventoryReserveNumber = GetSingleNodeText(node, "reserve_inventory_number");
            account.BillToAccountNumber = GetSingleNodeText(node, "bill_to_account_number");

            account.NumberOfBuilding = null;
            int temp1;
            if (int.TryParse(GetSingleNodeText(node, "BuildingCount"), out temp1))
            {
                account.NumberOfBuilding = temp1;
            }

            account.ProcessingCharge = null;
            decimal temp;
            if (decimal.TryParse(GetSingleNodeText(node, "ProcessingCharges"), out temp))
            {
                account.ProcessingCharge = temp;
            }

            account.ProcessingCharges2 = null;
            if (decimal.TryParse(GetSingleNodeText(node, "ProcessingCharges2"), out temp))
            {
                account.ProcessingCharges2 = temp;
            }

            account.ProcessingCharges3 = null;
            if (decimal.TryParse(GetSingleNodeText(node, "ProcessingCharges3"), out temp))
            {
                account.ProcessingCharges3 = temp;
            }

            account.SalesTax = null;
            float tempf;
            if (float.TryParse(GetSingleNodeText(node, "SalesTax"), out tempf))
            {
                account.SalesTax = tempf;
            }
        }

        private void GetOrgValuesForGeneralInfo(XmlElement orgObjectXml, Organization orgProfile)
        {
            var xml = orgObjectXml.SelectNodes("Organization/BTNextGen");
            if (xml != null)
            {
                foreach (XmlNode node in xml)
                {
                    if (node.Attributes == null) continue;

                    orgProfile.DefaultBookAccountId = GetSingleNodeText(node, "default_book_account_id");
                    orgProfile.DefaultEntAccountId = GetSingleNodeText(node, "default_entertainment_account_id");
                    orgProfile.DefaultVIPAccountId = GetSingleNodeText(node, "DefaultVIPAccount");
                    orgProfile.DefaultOneBoxAccountId = GetSingleNodeText(node, "default_onebox_account_id");

                    var arrAccountIds = GetArrayNodeValue(node, "account_list");
                    if (arrAccountIds != null && arrAccountIds.Count > 0)
                    {
                        orgProfile.Accounts = CSProfileController.Instance.GetAccounts(arrAccountIds);
                    }

                    orgProfile.DefaulteSuppliersAccountList = new List<string>();

                    var eSuppliers = GetArrayNodeValue(node, "default_esuppliers_account");
                    if (eSuppliers != null && eSuppliers.Count > 0)
                    {
                        orgProfile.DefaulteSuppliersAccountList.AddRange(eSuppliers);
                    }

                    orgProfile.AllWarehouse = GetSingleNodeText(node, "all_warehouse") == "1";

                    var ptList = GetArrayNodeValue(node, "review_type_list");
                    if (ptList.Count > 0)
                    {
                        orgProfile.ReviewTypeList = ptList.ToArray();
                    }

                    orgProfile.PersonalProductURL = GetSingleNodeText(node, "personal_prod_url");
                    orgProfile.ProductLookupIndex = GetSingleNodeText(node, "prod_lookup_index");
                    orgProfile.ProductSuffixLookup = GetSingleNodeText(node, "prod_suffix_lookup");
                    orgProfile.ISBNLookupCode = GetSingleNodeText(node, "isbn_lookup_code");
                    orgProfile.ISBNLinkDisplayed = GetSingleNodeText(node, "isbn_link_displayed");
                    orgProfile.ProductLookupDeactivated = GetSingleNodeText(node, "product_lookup_deactivated") == "1";
                    orgProfile.IsOCLCCatalogingPlusEnabled = GetSingleNodeText(node, "oclc_cataloging_plus_enabled").Equals("1") ? true : false;

                    orgProfile.EntertainmentProduct = GetSingleNodeText(node, "entertainment_product") == "1";
                    orgProfile.TableOfContents = GetSingleNodeText(node, "table_of_contents") == "1";

                    orgProfile.AVPersonalProductURL = GetSingleNodeText(node, "av_personal_prod_url");
                    orgProfile.AVProductLookupIndex = GetSingleNodeText(node, "av_prod_lookup_index");
                    orgProfile.AVProductSuffixLookup = GetSingleNodeText(node, "av_prod_suffix_lookup");
                    orgProfile.AVUseISBN = GetSingleNodeText(node, "av_use_isbn") == "1";
                    orgProfile.AVUseUPC14 = GetSingleNodeText(node, "av_use_upc_14") == "1";
                    orgProfile.AVProductLookupDeactivated = GetSingleNodeText(node, "av_product_lookup_deactivated") == "1";

                    orgProfile.ILSAcquisitionsEnabled = GetSingleNodeText(node, "ILS_acquisitions_enabled");
                    orgProfile.ILSAcquisitionsApiKey = GetSingleNodeText(node, "ILS_acquisitions_api_key");
                    orgProfile.ILSAcquisitionsApiPassphrase = GetSingleNodeText(node, "ILS_acquisitions_api_passphrase");
                    orgProfile.ILSAcquisitionsApiURL = GetSingleNodeText(node, "ILS_acquisitions_api_url");
                    orgProfile.ILSAcquisitionsUserId = GetSingleNodeText(node, "ILS_Login");
                }
            }
        }

        private void GetValuesForAccountInfo(XmlElement userObjectXml, UserProfile userProfile)
        {
            var xmlMyPreferences = userObjectXml.SelectNodes("/UserObject/AccountInfo");
            if (xmlMyPreferences != null)
            {
                foreach (XmlNode node in xmlMyPreferences)
                {
                    if (node.Attributes == null) continue;

                    userProfile.OrgId = GetSingleNodeText(node, "org_id");

                    var accountListString = GetArrayNodeValue(node, "account_view_orders");
                    if (accountListString != null && accountListString.Count > 0)
                    {
                        userProfile.AccountViewOrders = CSProfileController.Instance.GetAccounts(accountListString);
                    }

                    accountListString = GetArrayNodeValue(node, "account_create_carts");
                    if (accountListString != null && accountListString.Count > 0)
                    {
                        userProfile.AccountCreateCarts = CSProfileController.Instance.GetAccounts(accountListString);
                    }
                }
            }
        }

        private void GetValuesForMyPref(XmlElement userObjectXml, UserProfile userProfile)
        {
            var xmlMyPreferences = userObjectXml.SelectNodes("/UserObject/MyPreferences");
            if (xmlMyPreferences != null)
            {
                foreach (XmlNode node in xmlMyPreferences)
                {
                    if (node.Attributes == null) continue;

                    userProfile.DefaultDuplicateOrders = GetSingleNodeText(node, "default_duplicate_orders");
                    userProfile.DefaultDuplicateCarts = GetSingleNodeText(node, "default_duplicate_carts");
                    userProfile.HoldingsFlag = GetSingleNodeText(node, "holdings_flag");

                    var ptList = GetArrayNodeValue(node, "product_type_filter");
                    if (ptList.Count > 0)
                    {
                        userProfile.ProductTypeFilter = ptList.ToArray();
                    }

                    userProfile.MyReviewTypeIds = GetArrayNodeValue(node, "my_review_type_list");
                    userProfile.CartSortBy = GetSingleNodeText(node, "cart_sort_by");
                    userProfile.CartSortOrder = GetSingleNodeText(node, "cart_sort_order");

                    userProfile.AltFormatCartSortBy = GetSingleNodeText(node, "alt_format_cart_sort_by");
                    if (string.IsNullOrEmpty(userProfile.AltFormatCartSortBy))
                        userProfile.AltFormatCartSortBy = SearchFieldNameConstants.format;

                    userProfile.AltFormatCartSortOrder = GetSingleNodeText(node, "alt_format_cart_sort_order");
                    if (string.IsNullOrEmpty(userProfile.AltFormatCartSortOrder))
                        userProfile.AltFormatCartSortOrder = "Ascending";

                    userProfile.AltFormatSearchSortBy = GetSingleNodeText(node, "alt_format_search_sort_by");
                    if (string.IsNullOrEmpty(userProfile.AltFormatSearchSortBy))
                        userProfile.AltFormatSearchSortBy = SearchFieldNameConstants.format;

                    userProfile.AltFormatSearchSortOrder = GetSingleNodeText(node, "alt_format_search_sort_order");
                    if (string.IsNullOrEmpty(userProfile.AltFormatSearchSortOrder))
                        userProfile.AltFormatSearchSortOrder = "Ascending";
                }
            }
        }

        private void GetValuesForBtNextGen(XmlElement userObjectXml, UserProfile userProfile)
        {
            var xmlBtNextGen = userObjectXml.SelectNodes("/UserObject/BTNextGen");
            if (xmlBtNextGen != null)
            {
                foreach (XmlNode node in xmlBtNextGen)
                {
                    if (node.Attributes == null) continue;

                    userProfile.UserName = GetSingleNodeText(node, "user_name");
                    userProfile.DefaultBookAccountId = GetSingleNodeText(node, "default_book_account");
                    userProfile.DefaultEntAccountId = GetSingleNodeText(node, "default_entertainment_account");
                    userProfile.DefaultVIPAccountId = GetSingleNodeText(node, "DefaultVIPAccount");
                    userProfile.DefaultOneBoxAccountId = GetSingleNodeText(node, "default_onebox_account");
                    userProfile.IsBTEmployee = GetSingleNodeText(node, "is_bt_employee").Equals("1") ? true : false;

                    var qtyText = GetSingleNodeText(node, "default_quantity");
                    if (qtyText != "")
                    {
                        userProfile.DefaultQuantity = int.Parse(qtyText);
                    }

                    var ptList = GetArrayNodeValue(node, "product_type_list");
                    if (ptList.Count > 0)
                    {
                        userProfile.ProductTypeList = ptList.ToArray();
                    }

                    userProfile.BookExcludeFilter = GetSingleNodeText(node, "book_exclude_filter");
                    userProfile.BookIncludeFilter = GetSingleNodeText(node, "book_include_filter");

                    userProfile.MusicExcludeFilter = GetSingleNodeText(node, "music_exclude_filter");
                    userProfile.MusicIncludeFilter = GetSingleNodeText(node, "music_include_filter");

                    userProfile.MovieExcludeFilter = GetSingleNodeText(node, "movie_exclude_filter");
                    userProfile.MovieIncludeFilter = GetSingleNodeText(node, "movie_include_filter");

                    userProfile.DigitalExcludeFilter = GetSingleNodeText(node, "digital_exclude_filter");
                    userProfile.DigitalIncludeFilter = GetSingleNodeText(node, "digital_include_filter");

                    userProfile.DefaultESupplierAccountsList = GetArrayNodeValue(node, "default_esuppliers_account");
                }
            }
        }

        private string GetSingleNodeText(XmlNode parentNode, string nodeName)
        {
            var node = parentNode.SelectNodes(nodeName);
            if (node == null || node.Count == 0) return "";

            return node[0].InnerText;
        }

        private List<string> GetArrayNodeValue(XmlNode parentNode, string nodeName)
        {
            var result = new List<string>();

            var node = parentNode.SelectNodes(nodeName);
            if (node == null || node.Count == 0) return result;

            foreach (XmlNode xmlNode in node)
            {
                result.Add(xmlNode.InnerText);
            }
            return result;
        }

        #endregion
        
    }
}
