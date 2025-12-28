using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using BT.TS360API.Cache;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;

namespace BT.TS360API.Common.Inventory
{
    public class DetermineInventory: InventoryDAO
    {
        public static SiteContextObject SiteContext;

        /// <summary>
        /// Gets the inventory status for ItemDetails.
        /// </summary>
        /// <param name="searchArg">The search arg.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public string GetInventoryStatus(SearchResultInventoryStatusArg searchArg, string userId, string cartId, 
            MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            string skusList = null;
            string indexPositions = null;
            //var account = GetUserDefaultAccount(searchArg, userId);
            var account = InventoryHelper.GetUserDefaultAccountFromCartDetail(searchArg, userId, cartId);

            var warehousesAssigned = GetWareHousesAssigned(account, scMarketType, scCountryCode, scOrgId);

            var inventoryStatus = string.Empty;

            GetSkusList(searchArg.BTKey, searchArg.VariantId, searchArg.CatalogName, searchArg.Quantity, searchArg.Flag, searchArg.MarketType, 
                out skusList, out indexPositions);

            var quantityAvailable = 0;

            if (!string.IsNullOrEmpty(warehousesAssigned))
            {
                int last30DaysDemand;
                DataSet set = GetDataSet(CatalogConnectionString, skusList, indexPositions, warehousesAssigned, account, out last30DaysDemand, 
                    scMarketType, scCountryCode, scOrgId);

                var rows = set.Tables[0].Rows;

                for (var i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    quantityAvailable += int.Parse(row[PropertyName.InStockForRequest].ToString());
                }
            }
            string productflag = searchArg.Flag;
            if (string.IsNullOrEmpty(productflag))
            {
                productflag = string.Empty;
            }
            var prodType = CommonHelper.GetProductType(searchArg.ProductType);
            if (quantityAvailable > 0)
            {
                if (!string.IsNullOrEmpty(searchArg.ESupplier) && prodType == ProductType.Book)//Ebook
                    inventoryStatus = GetStatusName(OnhandInventoryStatus.Available);
                else
                    inventoryStatus = GetStatusName(OnhandInventoryStatus.InStock);
            }
            else if (!string.IsNullOrEmpty(searchArg.ESupplier) && prodType == ProductType.Book)//Ebook
            {
                var pubStatus = productflag.Replace(" ", "");
                DateTime pubDate = searchArg.PublishDate;
                if (string.IsNullOrEmpty(pubStatus))
                {
                    if ((pubDate != null) && (pubDate > DateTime.Now))
                        inventoryStatus = GetStatusName(OnhandInventoryStatus.AvailableToPreorder);
                    else
                        inventoryStatus = GetStatusName(OnhandInventoryStatus.AvailableToBackorder);
                }
                else
                {
                    switch (GetBTBPublisherStatus(pubStatus))
                    {
                        case BTBPublisherStatus.PublisherOutOfStock:
                            inventoryStatus = GetStatusName(OnhandInventoryStatus.AvailableToBackorder);
                            break;
                        case BTBPublisherStatus.NotYetPublished:
                            inventoryStatus = GetStatusName(OnhandInventoryStatus.AvailableToPreorder);
                            break;
                    }
                }
            }
            return inventoryStatus;
        }

        /// <summary>
        /// Gets the inventory status for searchResult + CartDetails.
        /// </summary>
        /// <param name="searchArgs">The search args.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<ItemData> GetInventoryStatus(List<SearchResultInventoryStatusArg> searchArgs, string userId, string cartId,
            MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            var itemDatas = new List<ItemData>();
            string skusList = null;
            string indexPositions = null;
            var bookArgs = new List<SearchResultInventoryStatusArg>();
            var entArgs = new List<SearchResultInventoryStatusArg>();

            foreach (var searchArg in searchArgs)
            {
                if (IsBookProduct(searchArg.ProductType))
                {
                    bookArgs.Add(searchArg);
                }
                else
                {
                    entArgs.Add(searchArg);
                }
                var itemData = new ItemData(string.Empty, searchArg.BTKey);
                itemDatas.Add(itemData);
            }

            if (bookArgs.Count > 0)
            {
                GetSkusList(bookArgs, out skusList, out indexPositions);
                GetInventoryStatus(bookArgs, itemDatas, userId, skusList, indexPositions, cartId, scMarketType, scCountryCode, scOrgId);
            }
            if (entArgs.Count > 0)
            {
                GetSkusList(entArgs, out skusList, out indexPositions);
                GetInventoryStatus(entArgs, itemDatas, userId, skusList, indexPositions, cartId, scMarketType, scCountryCode, scOrgId);
            }
            return itemDatas;
        }

        /// <summary>
        /// Get Onhand Inventory for ItemDetails
        /// </summary>
        /// <param name="searchArg">The search arg.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<InventoryStockStatus> GetOnhandInventory(SearchResultInventoryStatusArg searchArg, string userId, Account account,
            out int last30DaysDemand, MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            last30DaysDemand = 0;
            string skusList = null;
            string indexPositions = null;
            var inventoryStockStatuses = new List<InventoryStockStatus>();
            var dicBTBPubStatus = new Dictionary<string, BTBPublisherStatus>();
            var dicETEInactiveFlag = new Dictionary<string, BTEInactiveFlag>();
            var dicStatusName = new Dictionary<OnhandInventoryStatus, string>();

            var wareHouses = ProfileDAO.Instance.GetWareHouses();
            var dicAvailableWareHouses = GetWareHouses(wareHouses);

            GetSkusList(searchArg.BTKey, searchArg.VariantId, searchArg.CatalogName, searchArg.Quantity, searchArg.Flag,
                searchArg.MarketType, out skusList, out indexPositions);
            if (dicAvailableWareHouses.Count > 0)
            {
                var warehousesAssigned = GetWareHousesAssigned(account, scMarketType, scCountryCode, scOrgId);

                //if (!string.IsNullOrEmpty(warehousesAssigned))
                {
                    var availableWareHouses = string.Empty;
                    foreach (var dicAvailableWareHouse in dicAvailableWareHouses)
                    {
                        availableWareHouses += dicAvailableWareHouse.Key + GeneralConstants.Semicolon;
                    }

                    var set = GetDataSet(CatalogConnectionString, skusList, indexPositions, availableWareHouses, account, out last30DaysDemand,
                        scMarketType, scCountryCode, scOrgId);

                    var rows = set.Tables[0].Rows;

                    for (int i = 0; i < rows.Count; i++)
                    {
                        var row = rows[i];

                        var inventoryStockStatus = new InventoryStockStatus();

                        var wareHouse = row[PropertyName.WarehouseId].ToString();
                        inventoryStockStatus.LineItem = int.Parse(row[PropertyName.LineItem].ToString());
                        inventoryStockStatus.StockCondition = int.Parse(row[PropertyName.StockCondition].ToString());
                        inventoryStockStatus.InStockForRequest =
                            int.Parse(row[PropertyName.InStockForRequest].ToString());
                        inventoryStockStatus.QuantityAvailable =
                            int.Parse(row[PropertyName.InStockForRequest].ToString());
                        inventoryStockStatus.OnOrderQuantity =
                            int.Parse(row[PropertyName.OnOrderQuantity].ToString());
                        inventoryStockStatus.WareHouseCode = wareHouse;

                        inventoryStockStatus.InvDemandNumber = row[PropertyName.Last30DayDemand] == null ? 0 :
                            int.Parse(row[PropertyName.Last30DayDemand].ToString());

                        if (DetermineOnhandInventory(inventoryStockStatus, wareHouse, searchArg.ProductType, searchArg.Flag,
                                                     dicAvailableWareHouses, dicBTBPubStatus, dicETEInactiveFlag,
                                                     dicStatusName, warehousesAssigned, searchArg.PublishDate, searchArg.ESupplier, searchArg.ReportCode))
                        {
                            inventoryStockStatuses.Add(inventoryStockStatus);
                        }
                    }

                    //if (set.Tables.Count > 1)
                    //{
                    //    var rowsOfSecondTable = set.Tables[1].Rows;
                    //    foreach (DataRow dataRow in rowsOfSecondTable)
                    //    {
                    //        var btkey = dataRow["BTKey"].ToString();
                    //        if (string.Compare(btkey, searchArg.BTKey, StringComparison.OrdinalIgnoreCase) == 0)
                    //        {
                    //            if (!int.TryParse(dataRow["Last30DayDemand"].ToString(), out last30DaysDemand))
                    //            {
                    //                last30DaysDemand = 0;
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            return inventoryStockStatuses;
        }

        /// <summary>
        /// Gets the onhand inventory for submitOrder.
        /// </summary>
        /// <param name="searchArg">The search arg.</param>
        /// <param name="account">The account.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<InventoryStockStatus> GetOnhandInventory(List<SearchResultInventoryStatusArg> searchArg, Account account, string userId,
            MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            string skusList = null;
            string indexPositions = null;

            var inventoryStockStatuses = new List<InventoryStockStatus>();

            // Init values
            for (var i = 0; i < searchArg.Count; i++)
            {
                var inventoryStock = new InventoryStockStatus
                {
                    OnOrderQuantity = 0,
                    QuantityAvailable = 0,
                    InStockForRequest = 0
                };
                inventoryStockStatuses.Add(inventoryStock);
            }

            string availableWareHouses = string.Empty;
            if (account != null)
            {
                if (account.PrimaryWarehouse != null)
                {
                    var wareHouse = account.PrimaryWarehouse; //(Warehouse)account.PrimaryWarehouse.Target;
                    if (wareHouse.Code != null)
                    {
                        availableWareHouses += wareHouse.Code + GeneralConstants.Semicolon;
                    }
                }
                if (account.SecondaryWarehouse != null)
                {
                    var wareHouse = account.SecondaryWarehouse; //(Warehouse)account.SecondaryWarehouse.Target;
                    if (wareHouse.Code != null)
                    {
                        availableWareHouses += wareHouse.Code + GeneralConstants.Semicolon;
                    }
                }
            }

            if (!string.IsNullOrEmpty(availableWareHouses))
            {
                GetSkusList(searchArg, out skusList, out indexPositions);

                int last30DaysDemand;
                DataSet set = GetDataSet(CatalogConnectionString, skusList, indexPositions, availableWareHouses, account, out last30DaysDemand,
                    scMarketType, scCountryCode, scOrgId);

                DataRowCollection rows = set.Tables[0].Rows;

                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i];
                    var index = int.Parse(row[PropertyName.LineItem].ToString());

                    if (index >= 0)
                    {
                        var wareHouse = row[PropertyName.WarehouseId].ToString();

                        if (CheckValidWareHouseCode(wareHouse))
                        {
                            inventoryStockStatuses[index].OnOrderQuantity +=
                                int.Parse(row[PropertyName.OnOrderQuantity].ToString());
                            inventoryStockStatuses[index].QuantityAvailable +=
                                int.Parse(row[PropertyName.InStockForRequest].ToString());
                            inventoryStockStatuses[index].WareHouseCode = wareHouse;
                            inventoryStockStatuses[index].LineItem = int.Parse(row[PropertyName.LineItem].ToString());
                            inventoryStockStatuses[index].StockCondition =
                                int.Parse(row[PropertyName.StockCondition].ToString());
                            inventoryStockStatuses[index].InStockForRequest +=
                                int.Parse(row[PropertyName.InStockForRequest].ToString());

                        }
                    }
                }
            }

            return inventoryStockStatuses;
        }

        #region Private Methods
        /// <summary>
        /// Checks the valid ware house code.
        /// </summary>
        /// <param name="wareHouse">The ware house.</param>
        /// <returns></returns>
        private bool CheckValidWareHouseCode(string wareHouse)
        {
            wareHouse = wareHouse.ToUpper();
            return wareHouse == "COM" || wareHouse == "MOM" || wareHouse == "SOM" || wareHouse == "RNO" || wareHouse == "REN" ||
                wareHouse == "VIM" || wareHouse == "VIE" || wareHouse == "VIP";
        }

        /// <summary>
        /// Gets the inventory status.
        /// </summary>
        /// <param name="searchArgs">The search args.</param>
        /// <param name="itemDatas">The item datas.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="skusList">The skus list.</param>
        /// <param name="indexPositions">The index positions.</param>
        private void GetInventoryStatus(List<SearchResultInventoryStatusArg> searchArgs, List<ItemData> itemDatas,
            string userId, string skusList, string indexPositions, string cartId, MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            //var account = GetUserDefaultAccount(searchArgs[0], userId);
            var account = InventoryHelper.GetUserDefaultAccountFromCartDetail(searchArgs[0], userId, cartId);
            var warehousesAssigned = GetWareHousesAssigned(account, scMarketType, scCountryCode, scOrgId);
            var dicBTBPubStatus = new Dictionary<string, BTBPublisherStatus>();
            var dicETEInactiveFlag = new Dictionary<string, BTEInactiveFlag>();
            var dicStatusName = new Dictionary<OnhandInventoryStatus, string>();
            var quantityAvailable = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(warehousesAssigned))
            {
                int last30DaysDemand;
                var temWhs = warehousesAssigned;
                if (warehousesAssigned == "REN")
                {
                    temWhs = "RNO";
                }
                var dictRows = CachingController.Instance.Read(CacheKeyConstant.InventoryCacheKey) as Dictionary<string, List<DataRow>>;
                    //VelocityCacheManager.Read(CacheKeyConstant.InventoryCacheKey) as Dictionary<string, List<DataRow>>;
                if (dictRows != null && dictRows.Count > 0)
                {
                    SetQuantityAvaliable(dictRows, temWhs, quantityAvailable);
                }
                else
                {
                    var set = GetDataSet(CatalogConnectionString, skusList, indexPositions, warehousesAssigned, account, out last30DaysDemand,
                        scMarketType, scCountryCode, scOrgId);
                    var dictDatRows = CommonHelper.ConvertInventoryDataToDictionary(set);
                    SetQuantityAvaliable(dictDatRows, temWhs, quantityAvailable);
                }

                for (var i = 0; i < searchArgs.Count; i++)
                {
                    var btKey = searchArgs[i].BTKey;
                    if (btKey == null || !quantityAvailable.ContainsKey(btKey)) continue;
                    var inventoryStatus = DetermineOnhandStatus(quantityAvailable[btKey], searchArgs[i].ProductType, searchArgs[i].ESupplier, searchArgs[i].ReportCode,
                                                                searchArgs[i].Flag, dicBTBPubStatus, dicETEInactiveFlag,
                                                                searchArgs[i].PublishDate);
                    foreach (var data in itemDatas)
                    {
                        if (data.ItemDataText == searchArgs[i].BTKey)
                        {
                            data.ItemDataValue = inventoryStatus == OnhandInventoryStatus.EmptyStock
                                                     ? string.Empty
                                                     : GetStatusName(dicStatusName, inventoryStatus);
                            break;
                        }
                    }
                }
            }
        }

        private static void SetQuantityAvaliable(Dictionary<string, List<DataRow>> dictRows, string temWhs, Dictionary<string, int> quantityAvailable)
        {
            foreach (var key in dictRows.Keys)
            {
                var listDr = dictRows[key];
                foreach (var dataRow in listDr)
                {
                    var whs = dataRow[PropertyName.WarehouseId].ToString().Trim();
                    if (temWhs.IndexOf(whs, StringComparison.OrdinalIgnoreCase) == -1) continue;
                    if (!quantityAvailable.ContainsKey(key))
                    {
                        quantityAvailable.Add(key, int.Parse(dataRow[PropertyName.InStockForRequest].ToString()));
                    }
                    else
                    {
                        var qty = quantityAvailable[key];
                        quantityAvailable[key] = qty + int.Parse(dataRow[PropertyName.InStockForRequest].ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Determines the onhand inventory.
        /// </summary>
        /// <param name="inventoryStockStatus">The inventory stock status.</param>
        /// <param name="wareHouse">The ware house.</param>
        /// <param name="productType">Type of the product.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="dicAvailableWareHouses">The dic available ware houses.</param>
        /// <param name="dicBTBPublisherStatus">The dic BTB publisher status.</param>
        /// <param name="dicBTEInactiveFlag">The dic BTE inactive flag.</param>
        /// <param name="dicStatusName">Name of the dic status.</param>
        /// <param name="warehousesAssigned">The warehouses assigned.</param>
        /// <returns></returns>
        private bool DetermineOnhandInventory(InventoryStockStatus inventoryStockStatus, string wareHouse,
                                              string productType, string flag,
                                              Dictionary<string, string> dicAvailableWareHouses,
                                              Dictionary<string, BTBPublisherStatus> dicBTBPublisherStatus,
                                              Dictionary<string, BTEInactiveFlag> dicBTEInactiveFlag,
                                              Dictionary<OnhandInventoryStatus, string> dicStatusName,
                                              string warehousesAssigned, DateTime pubDate,
                                              string eSupplier, string reportCode)
        {
            bool result = true;

            if (string.IsNullOrEmpty(flag))
                flag = string.Empty;

            if (CheckValidWareHouseCode(wareHouse))
            {
                if (dicAvailableWareHouses.ContainsKey(wareHouse))
                {
                    inventoryStockStatus.WareHouse = dicAvailableWareHouses[wareHouse];
                }

                if (inventoryStockStatus.QuantityAvailable > 0 && !InventoryHelper.IsVIPWarehouse(wareHouse))
                {
                    inventoryStockStatus.OnHandInventory = inventoryStockStatus.QuantityAvailable.ToString();
                }
                else
                {
                    if (warehousesAssigned.Contains(wareHouse))
                    {

                        // Process for VIP
                        if (InventoryHelper.IsVIPWarehouse(wareHouse))
                        {
                            inventoryStockStatus.OnHandInventory = inventoryStockStatus.QuantityAvailable.ToString(); //(inventoryStockStatus.OnOrderQuantity > 0) ? inventoryStockStatus.QuantityAvailable.ToString() : "0";
                        }

                        // Process for Book
                        else if (IsBookProduct(productType))
                        {
                            var pubStatus = flag.Replace(" ", "");
                            if (string.IsNullOrEmpty(pubStatus))
                            {
                                if ((pubDate != null) && (pubDate > DateTime.Now))
                                {
                                    //inventoryStockStatus.OnHandInventory = GetStatusName(dicStatusName,
                                    //                                                     OnhandInventoryStatus.AvailableToPreorder);
                                    inventoryStockStatus.OnHandInventory = GetStatusNameItemDetails(dicStatusName,
                                                                                         OnhandInventoryStatus.AvailableToPreorder);

                                }
                                else
                                {
                                    inventoryStockStatus.OnHandInventory = GetStatusNameItemDetails(dicStatusName,
                                                                                         OnhandInventoryStatus.
                                                                                             AvailableToBackorder);
                                }
                            }
                            else
                            {
                                switch (GetBTBPublisherStatus(pubStatus, dicBTBPublisherStatus))
                                {
                                    case BTBPublisherStatus.PublisherOutOfStock:
                                        inventoryStockStatus.OnHandInventory = GetStatusNameItemDetails(dicStatusName,
                                                                                             OnhandInventoryStatus.AvailableToBackorder);
                                        break;
                                    case BTBPublisherStatus.NotYetPublished:
                                        inventoryStockStatus.OnHandInventory = GetStatusNameItemDetails(dicStatusName,
                                                                                             OnhandInventoryStatus.AvailableToPreorder);
                                        break;
                                    default:
                                        if (inventoryStockStatus.OnOrderQuantity > 0)
                                        {
                                            inventoryStockStatus.OnHandInventory =
                                                inventoryStockStatus.QuantityAvailable.ToString();
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            // Process for Entertainment
                            var inactiveFlag = flag.Replace(" ", "");
                            if (string.IsNullOrEmpty(inactiveFlag))
                            {
                                //Maybe don't need this case
                                if ((pubDate != null) && (pubDate > DateTime.Now))
                                {
                                    inventoryStockStatus.OnHandInventory = GetStatusNameItemDetails(dicStatusName,
                                                                                         OnhandInventoryStatus.AvailableToPreorder);
                                }
                                else
                                {
                                    inventoryStockStatus.OnHandInventory = GetStatusNameItemDetails(dicStatusName,
                                                                                         OnhandInventoryStatus.
                                                                                             AvailableToBackorder);
                                }
                            }
                            else
                            {
                                switch (GetBTEInactiveFlag(inactiveFlag, dicBTEInactiveFlag))
                                {
                                    case BTEInactiveFlag.SupplierOutOfStock:
                                        inventoryStockStatus.OnHandInventory = GetStatusNameItemDetails(dicStatusName,
                                                                                             OnhandInventoryStatus.AvailableToBackorder);
                                        break;
                                    case BTEInactiveFlag.NotYetPublished:
                                        inventoryStockStatus.OnHandInventory = GetStatusNameItemDetails(dicStatusName,
                                                                                             OnhandInventoryStatus.AvailableToPreorder);
                                        break;
                                    default:
                                        if (inventoryStockStatus.OnOrderQuantity > 0)
                                        {
                                            inventoryStockStatus.OnHandInventory =
                                                inventoryStockStatus.QuantityAvailable.ToString();
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (inventoryStockStatus.OnOrderQuantity > 0 && !InventoryHelper.IsVIPWarehouse(wareHouse))
                        {
                            inventoryStockStatus.OnHandInventory = inventoryStockStatus.QuantityAvailable.ToString();
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Gets the BTB publisher status.
        /// </summary>
        /// <param name="pubStatus">The pub status.</param>
        /// <param name="dicBTBPublisherStatus">The dic BTB publisher status.</param>
        /// <returns></returns>
        private BTBPublisherStatus GetBTBPublisherStatus(string pubStatus, Dictionary<string, BTBPublisherStatus> dicBTBPublisherStatus)
        {
            if (!dicBTBPublisherStatus.ContainsKey(pubStatus))
                dicBTBPublisherStatus.Add(pubStatus, GetBTBPublisherStatus(pubStatus));
            return dicBTBPublisherStatus[pubStatus];
        }

        private BTBPublisherStatus GetBTBPublisherStatus(string pubStatus)
        {
            BTBPublisherStatus btbPubStatus;

            if (!CommonHelper.TryParseEnum(pubStatus, out btbPubStatus))
                btbPubStatus = BTBPublisherStatus.NotDefined;

            return btbPubStatus;
        }

        /// <summary>
        /// Gets the BTE inactive flag.
        /// </summary>
        /// <param name="inActiveFlag">The in active flag.</param>
        /// <param name="dicBTEInactiveFlag">The dic BTE inactive flag.</param>
        /// <returns></returns>
        private BTEInactiveFlag GetBTEInactiveFlag(string inActiveFlag, Dictionary<string, BTEInactiveFlag> dicBTEInactiveFlag)
        {
            if (!dicBTEInactiveFlag.ContainsKey(inActiveFlag))
            {
                BTEInactiveFlag bteInactiveFlag;

                if (!CommonHelper.TryParseEnum(inActiveFlag, out bteInactiveFlag))
                    bteInactiveFlag = BTEInactiveFlag.NotDefined;

                dicBTEInactiveFlag.Add(inActiveFlag, bteInactiveFlag);
            }
            return dicBTEInactiveFlag[inActiveFlag];
        }

        /// <summary>
        /// Gets the name of the status.
        /// </summary>
        /// <param name="dicStatusName">Name of the dic status.</param>
        /// <param name="inventoryStatus">The inventory status.</param>
        /// <returns></returns>
        public string GetStatusName(Dictionary<OnhandInventoryStatus, string> dicStatusName, OnhandInventoryStatus inventoryStatus)
        {
            if (!dicStatusName.ContainsKey(inventoryStatus))
            {
                dicStatusName.Add(inventoryStatus, GetStatusName(inventoryStatus));
            }
            return dicStatusName[inventoryStatus];
        }

        /// <summary>
        /// Gets the name of the status.
        /// </summary>
        /// <param name="dicStatusName">Name of the dic status.</param>
        /// <param name="inventoryStatus">The inventory status.</param>
        /// <returns></returns>
        private string GetStatusNameItemDetails(Dictionary<OnhandInventoryStatus, string> dicStatusName, OnhandInventoryStatus inventoryStatus)
        {
            if (!dicStatusName.ContainsKey(inventoryStatus))
            {
                dicStatusName.Add(inventoryStatus, GetStatusNameItemDetails(inventoryStatus));
            }
            return dicStatusName[inventoryStatus];
        }

        /// <summary>
        /// Determine Onhand Status
        /// </summary>
        /// <param name="quantiyAvailable"></param>
        /// <param name="productType"></param>
        /// <param name="productflag"></param>
        /// <returns></returns>
        public OnhandInventoryStatus DetermineOnhandStatus(int quantiyAvailable, string productType, string eSupplier, string reportCode, string productflag,
                                                             Dictionary<string, BTBPublisherStatus> dicBTBPublisherStatus,
                                                             Dictionary<string, BTEInactiveFlag> dicBTEInactiveFlag,
                                                                DateTime pubDate)
        {

            OnhandInventoryStatus inventoryStatus = OnhandInventoryStatus.EmptyStock;

            if (string.IsNullOrEmpty(productflag))
            {
                productflag = string.Empty;
            }

            if (quantiyAvailable > 0)
            {
                if (!string.IsNullOrEmpty(eSupplier) && IsBookProduct(productType))
                    inventoryStatus = OnhandInventoryStatus.Available;
                else
                    inventoryStatus = OnhandInventoryStatus.InStock;
            }
            else
            {
                // Process for Book
                if (IsBookProduct(productType))
                {
                    var pubStatus = productflag.Replace(" ", "");
                    if (string.IsNullOrEmpty(pubStatus))
                    {
                        if ((pubDate != null) && (pubDate > DateTime.Now))
                        {
                            inventoryStatus = OnhandInventoryStatus.AvailableToPreorder;
                        }
                        else
                        {
                            inventoryStatus = OnhandInventoryStatus.AvailableToBackorder;
                        }
                    }
                    else
                    {
                        var btbPublisherStatus = GetBTBPublisherStatus(pubStatus, dicBTBPublisherStatus);
                        switch (btbPublisherStatus)
                        {
                            case BTBPublisherStatus.PublisherOutOfStock:
                                inventoryStatus = OnhandInventoryStatus.AvailableToBackorder;
                                break;
                            case BTBPublisherStatus.NotYetPublished:
                                inventoryStatus = OnhandInventoryStatus.AvailableToPreorder;
                                break;
                        }
                    }
                }

                else
                {
                    // Process for Entertainment
                    var inactiveFlag = productflag.Replace(" ", "");
                    if (string.IsNullOrEmpty(inactiveFlag))
                    {
                        if (pubDate == null || pubDate > DateTime.Now || pubDate.Year == DateTime.MinValue.Year)
                        {
                            inventoryStatus = OnhandInventoryStatus.AvailableToPreorder;
                        }
                        else
                        {
                            inventoryStatus = OnhandInventoryStatus.AvailableToBackorder;
                        }
                    }
                    else
                    {
                        var bteInactiveFlag = GetBTEInactiveFlag(inactiveFlag, dicBTEInactiveFlag);
                        switch (bteInactiveFlag)
                        {
                            case BTEInactiveFlag.SupplierOutOfStock:
                                inventoryStatus = OnhandInventoryStatus.AvailableToBackorder;
                                break;
                            case BTEInactiveFlag.NotYetPublished:
                                inventoryStatus = OnhandInventoryStatus.AvailableToPreorder;
                                break;
                        }
                    }
                }
            }

            return inventoryStatus;
        }

        /// <summary>
        /// Get Data Set
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="skusList"></param>
        /// <param name="indexPositions"></param>
        /// <param name="warehousesAssigned"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private DataSet GetDataSet(string connectionString, string skusList, string indexPositions,
                                   string warehousesAssigned, Account account, out int last30DaysDemand, 
            MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            const string FALSE = "0";
            const string TRUE = "1";
            //DataSet dataSet = null;

            //var spName = GeneralConstants.StoreProcOnHandInventory;

            ConnectionString = connectionString + GeneralConstants.PersistSecurity;

            //var isTolas = false;
            var checkLEReserve = FALSE;
            var accountInventoryType = string.Empty;
            var inventoryReserveNumber = string.Empty;
            //var checkReserveFlag = false;
            if (account != null)
            {
                //if (account.IsTOLAS != null)
                //{
                //    isTolas = (bool)account.IsTOLAS;
                //}
                if (account.CheckLEReserve != null && account.CheckLEReserve.Value)
                {
                    checkLEReserve = TRUE;
                }
                if (account.AccountInventoryType != null)
                {
                    accountInventoryType = account.AccountInventoryType;
                }
                if (account.InventoryReserveNumber != null)
                {
                    inventoryReserveNumber = account.InventoryReserveNumber;
                }
                //if (account.CheckReserveFlag != null)
                //{
                //    checkReserveFlag = (bool)account.CheckReserveFlag;
                //}
            }
            DataSet dsInventory = ProductDAO.Instance.GetInventory(skusList, BuildAssignedWarehouses(warehousesAssigned), checkLEReserve,
                                             accountInventoryType, inventoryReserveNumber, out last30DaysDemand);

            var marketType = scMarketType.HasValue ? scMarketType.Value : MarketType.Any;

            if (InventoryHelper.IsDisplaySuperWarehouse(marketType, scCountryCode, scOrgId))
            {
                InventoryHelper.AddSuperWarehouse(ref dsInventory);
            }
            return dsInventory;
        }

        /// <summary>
        /// Removes the duplicate ware house.
        /// </summary>
        /// <param name="warehousesAssigned">The warehouses assigned.</param>
        /// <returns></returns>
        private string BuildAssignedWarehouses(string warehousesAssigned)
        {
            if (!string.IsNullOrEmpty(warehousesAssigned))
            {
                var splitStr = warehousesAssigned.Split(GeneralConstants.Semicolon);
                var listWarehouse = new List<string>(splitStr);
                bool needRegenerate = false;

                //REN and RNO point to the same wareHouse => need to remove them
                if (listWarehouse.Contains("REN") && listWarehouse.Contains("RNO"))
                {
                    listWarehouse.Remove("REN");
                    needRegenerate = true;
                }

                if (listWarehouse.Contains(InventoryWareHouseCode.SUP))
                {
                    listWarehouse.Remove(InventoryWareHouseCode.SUP);
                    needRegenerate = true;
                }

                if (needRegenerate)
                {
                    //Regenerate warehouse list
                    warehousesAssigned = string.Empty;
                    foreach (var wareHouse in listWarehouse)
                    {
                        warehousesAssigned += wareHouse + GeneralConstants.Semicolon;
                    }
                }
            }
            return warehousesAssigned;
        }

        private bool IsBookProduct(string productType)
        {
            return productType == ProductTypeConstants.Book;
        }

        ///// <summary>
        ///// Gets the account.
        ///// </summary>
        ///// <param name="accountId">The account id.</param>
        ///// <returns></returns>
        //private static Account GetAccount(string accountId)
        //{
        //    Account account = null;
        //    if (!string.IsNullOrEmpty(accountId))
        //    {
        //        var profileControllerForAdmin = AdministrationProfileController.Current;
        //        profileControllerForAdmin.AccountRelated.PrimaryWarehouseNeeded = true;
        //        profileControllerForAdmin.AccountRelated.SecondaryWarehouseNeeded = true;
        //        account = profileControllerForAdmin.GetAccountById(accountId);
        //    }

        //    return account;
        //}

        /// <summary>
        /// Gets the ware houses assigned.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        private string GetWareHousesAssigned(Account account, MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            var warehousesAssigned = string.Empty;

            if (account != null)
            {
                var primaryWareHouse = string.Empty;
                var secondaryWareHouse = string.Empty;
                Warehouse wareHouse;

                if (account.PrimaryWarehouse != null)
                {
                    wareHouse = account.PrimaryWarehouse;
                    if (wareHouse != null)
                    {
                        primaryWareHouse = wareHouse.Code;
                    }
                    if (!string.IsNullOrEmpty(primaryWareHouse))
                    {
                        warehousesAssigned += primaryWareHouse + GeneralConstants.Semicolon;
                    }
                }

                if (account.SecondaryWarehouse != null)
                {
                    wareHouse = account.SecondaryWarehouse;
                    if (wareHouse != null)
                    {
                        secondaryWareHouse = wareHouse.Code;
                    }
                    if (!string.IsNullOrEmpty(secondaryWareHouse))
                    {
                        warehousesAssigned += secondaryWareHouse + GeneralConstants.Semicolon;
                    }
                }
            }
            else
            {
                //If user and its organization has no default account => use all 4 warehouse 
                var wareHouses = ProfileDAO.Instance.GetWareHouses();
                foreach (var wareHouse in wareHouses)
                {
                    if (!InventoryHelper.IsVIPWarehouse(wareHouse.Code))
                    {
                        warehousesAssigned += wareHouse.Code + GeneralConstants.Semicolon;
                    }
                }
            }

            var marketType = scMarketType.HasValue ? scMarketType.Value : MarketType.Any;

            if (InventoryHelper.IsDisplaySuperWarehouse(marketType, scCountryCode, scOrgId))
            {
                warehousesAssigned += InventoryWareHouseCode.SUP + GeneralConstants.Semicolon;
            }
            else if (InventoryHelper.IsDisplayVIPWarehouse(marketType, scCountryCode, scOrgId))
            {
                warehousesAssigned += InventoryWareHouseCode.VIE + GeneralConstants.Semicolon;
                warehousesAssigned += InventoryWareHouseCode.VIM + GeneralConstants.Semicolon;
            }

            return warehousesAssigned;
        }

        private string GetWareHousesAssignedForSearchResultsPage(Account account, List<Warehouse> wareHouses, 
            MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            var warehousesAssigned = string.Empty;

            if (account != null)
            {
                var primaryWareHouse = string.Empty;
                var secondaryWareHouse = string.Empty;
                Warehouse wareHouse;

                if (!string.IsNullOrEmpty(account.PrimaryWarehouseCode))
                {
                    primaryWareHouse = account.PrimaryWarehouseCode;
                    //wareHouse = account.PrimaryWarehouse.Target;
                    //if (wareHouse != null)
                    //{
                    //    primaryWareHouse = wareHouse.Code;
                    //}
                    if (!string.IsNullOrEmpty(primaryWareHouse))
                    {
                        warehousesAssigned += primaryWareHouse + GeneralConstants.Semicolon;
                    }
                }

                if (!string.IsNullOrEmpty(account.SecondaryWarehouseCode))
                {
                    secondaryWareHouse = account.SecondaryWarehouseCode;
                    //wareHouse = account.SecondaryWarehouse.Target;
                    //if (wareHouse != null)
                    //{
                    //    secondaryWareHouse = wareHouse.Code;
                    //}
                    if (!string.IsNullOrEmpty(secondaryWareHouse))
                    {
                        warehousesAssigned += secondaryWareHouse + GeneralConstants.Semicolon;
                    }
                }

            }
            else
            {
                //If user and its organization has no default account => use all 4 warehouse 
                foreach (var wareHouse in wareHouses)
                {
                    if (!InventoryHelper.IsVIPWarehouse(wareHouse.Code))
                    {
                        warehousesAssigned += wareHouse.Code + GeneralConstants.Semicolon;
                    }
                }
            }

            var marketType = scMarketType.HasValue ? scMarketType.Value : MarketType.Any;

            if (InventoryHelper.IsDisplaySuperWarehouse(marketType, scCountryCode, scOrgId))
            {
                warehousesAssigned += InventoryWareHouseCode.SUP + GeneralConstants.Semicolon;
            }
            else if (InventoryHelper.IsDisplayVIPWarehouse(marketType, scCountryCode, scOrgId))
            {
                warehousesAssigned += InventoryWareHouseCode.VIE + GeneralConstants.Semicolon;
                warehousesAssigned += InventoryWareHouseCode.VIM + GeneralConstants.Semicolon;
            }

            return warehousesAssigned;
        }

        /// <summary>
        /// Gets the ware houses.
        /// </summary>
        /// <param name="wareHouses">The ware houses.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetWareHouses(IEnumerable<Warehouse> wareHouses)
        {
            var result = new Dictionary<string, string>();
            foreach (var wareHouse in wareHouses)
            {
                string id = wareHouse.Id;
                string name = wareHouse.Description;

                result.Add(id, name);
            }

            result.Add(InventoryWareHouseCode.SUP, InventoryWareHouseCode.SUP);

            return result;
        }

        public string GetAllWarehouse(List<Warehouse> warehouses)
        {
            return warehouses.Aggregate(string.Empty, (current, warehouse) => current + (warehouse.Code + GeneralConstants.Semicolon));
        }

        public List<InventoryStockStatus> GetOnhandInventoryForSearchResultPage(Account account, List<Warehouse> wareHouses,
            List<DataRow> rows, string productType, string flag, DateTime publishDate, string eSupplier, string reportCode,
            MarketType? scMarketType, string scCountryCode, string scOrgId)
        {
            var inventoryStockStatuses = new List<InventoryStockStatus>();
            var dicBTBPubStatus = new Dictionary<string, BTBPublisherStatus>();
            var dicETEInactiveFlag = new Dictionary<string, BTEInactiveFlag>();
            var dicStatusName = new Dictionary<OnhandInventoryStatus, string>();

            var dicAvailableWareHouses = GetWareHouses(wareHouses);

            //GetSkusList(searchArg.BTKey, searchArg.VariantId, searchArg.CatalogName, searchArg.Quantity, searchArg.Flag, searchArg.MarketType, out skusList, out indexPositions);
            if (dicAvailableWareHouses != null && dicAvailableWareHouses.Count > 0)
            {
                var warehousesAssigned = GetWareHousesAssignedForSearchResultsPage(account, wareHouses, scMarketType, scCountryCode, scOrgId);

                if (rows != null)
                    foreach (var row in rows)
                    {
                        var inventoryStockStatus = new InventoryStockStatus();

                        var wareHouse = row[PropertyName.WarehouseId].ToString();
                        inventoryStockStatus.LineItem = int.Parse(row[PropertyName.LineItem].ToString());
                        inventoryStockStatus.StockCondition = int.Parse(row[PropertyName.StockCondition].ToString());
                        inventoryStockStatus.InStockForRequest =
                            int.Parse(row[PropertyName.InStockForRequest].ToString());
                        inventoryStockStatus.QuantityAvailable =
                            int.Parse(row[PropertyName.InStockForRequest].ToString());
                        inventoryStockStatus.OnOrderQuantity =
                            int.Parse(row[PropertyName.OnOrderQuantity].ToString());
                        inventoryStockStatus.WareHouseCode = wareHouse;

                        inventoryStockStatus.InvDemandNumber = int.Parse(row["Last30DayDemand"].ToString());

                        if (DetermineOnhandInventory(inventoryStockStatus, wareHouse, productType, flag,
                                                     dicAvailableWareHouses, dicBTBPubStatus, dicETEInactiveFlag,
                                                     dicStatusName, warehousesAssigned, publishDate, eSupplier, reportCode))
                        {
                            inventoryStockStatuses.Add(inventoryStockStatus);
                        }
                    }
            }
            return inventoryStockStatuses;
        }

        /// <summary>
        /// Gets the skus list.
        /// </summary>
        /// <param name="searchArgs">The search args.</param>
        /// <param name="skusList">The skus list.</param>
        /// <param name="indexPositions">The index positions.</param>
        private static void GetSkusList(List<SearchResultInventoryStatusArg> searchArgs, out string skusList,
                                        out string indexPositions)
        {
            var skusBuilder = new StringBuilder();
            var indexBuilder = new StringBuilder();
            if (searchArgs == null)
            {
                skusList = string.Empty;
                indexPositions = string.Empty;
                return;
            }
            for (int i = 0; i < searchArgs.Count; i++)
            {
                string catalogName = searchArgs[i].CatalogName;
                string productId = searchArgs[i].BTKey;
                string variantId = searchArgs[i].VariantId;
                decimal quantity = searchArgs[i].Quantity;
                string publishStatus = searchArgs[i].Flag;
                string martketType = searchArgs[i].MarketType;

                string strQuantity = Math.Round(quantity).ToString(CultureInfo.InvariantCulture);
                skusBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1},{2},{3},{4},{5},",
                                         new object[]
                                             {
                                                 catalogName ?? string.Empty,
                                                 productId ?? string.Empty,
                                                 variantId ?? string.Empty,
                                                 strQuantity,
                                                 publishStatus ?? string.Empty,
                                                 martketType ?? string.Empty
                                             });
                indexBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1},{2},{3},{4},{5},",
                                          new object[]
                                              {
                                                  catalogName !=null ? catalogName.Length : 0,
                                                  productId !=null ? productId.Length : 0,
                                                  variantId !=null ? variantId.Length : 0,
                                                  strQuantity.Length,
                                                  publishStatus !=null ? publishStatus.Length : 0,
                                                  martketType !=null ? martketType.Length : 0
                                              });
            }

            skusList = skusBuilder.ToString();
            indexPositions = indexBuilder.ToString();
        }

        /// <summary>
        /// Gets the skus list.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="variantId">The variant id.</param>
        /// <param name="catalogName">Name of the catalog.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="marketType"></param>
        /// <param name="skusList">The skus list.</param>
        /// <param name="indexPositions">The index positions.</param>
        /// <param name="publishStatus"></param>
        private static void GetSkusList(string productId, string variantId, string catalogName, decimal quantity, string publishStatus, string marketType,
                                        out string skusList, out string indexPositions)
        {
            var skusBuilder = new StringBuilder();
            var indexBuilder = new StringBuilder();

            decimal num = quantity;

            string invariantCulture = Math.Round(num).ToString(CultureInfo.InvariantCulture);
            skusBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1},{2},{3},{4},{5},",
                                 new object[]
                                     {
                                         catalogName ?? string.Empty,
                                         productId ?? string.Empty,
                                         variantId ?? string.Empty,
                                         invariantCulture,
                                         publishStatus ?? string.Empty,
                                         marketType ?? string.Empty
                                     });

            indexBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1},{2},{3},{4},{5},",
                                  new object[]
                                      {
                                          catalogName !=null ? catalogName.Length : 0,
                                          productId !=null ? productId.Length : 0,
                                          variantId !=null ? variantId.Length : 0,
                                          invariantCulture.Length,
                                          publishStatus !=null ? publishStatus.Length : 0,
                                          marketType !=null ? marketType.Length : 0
                                      });

            skusList = skusBuilder.ToString();
            indexPositions = indexBuilder.ToString();
        }

        /// <summary>
        /// Get Status Name From Site Term
        /// </summary>
        /// <param name="inventoryStatus"></param>
        /// <returns></returns>
        private string GetStatusName(OnhandInventoryStatus inventoryStatus)
        {
            if (string.IsNullOrEmpty(inventoryStatus.ToString()))
                return string.Empty;
            {
                return SiteTermHelper.Instance.GetSiteTermName(SiteTermName.OnhandInventoryStatus,
                    inventoryStatus.ToString());
                //return ProfileController.Current.GetSiteTermName(SiteTermName.OnhandInventoryStatus,
                //                                                 inventoryStatus.ToString());

            }
        }

        private string GetStatusNameItemDetails(OnhandInventoryStatus inventoryStatus)
        {
            if (string.IsNullOrEmpty(inventoryStatus.ToString()))
                return string.Empty;
            {
                return SiteTermHelper.Instance.GetSiteTermName(SiteTermName.OnhandInventoryStatusItemDetails,
                                                                 inventoryStatus.ToString());
                //return ProfileController.Current.GetSiteTermName(SiteTermName.OnhandInventoryStatusItemDetails,
                //                                                 inventoryStatus.ToString());
            }
        }

        /// <summary>
        /// Gets the inventory status.
        /// </summary>
        /// <param name="searchArgs">The search args.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="allWarehouses"></param>
        /// <param name="account"></param>
        /// <param name="dicAvailableWarehouses"></param>
        public List<List<InventoryStockStatus>> GetInventoryResults(List<SearchResultInventoryStatusArg> searchArgs,
            string userId, Dictionary<string, string> dicAvailableWarehouses, string allWarehouses, out Account account, out DataSet data, 
            MarketType? scMarketType, string scCountryCode, string scOrgId, string cartId = "")
        {
            string skusList = null;
            string indexPositions = null;
            GetSkusList(searchArgs, out skusList, out indexPositions);

            account = InventoryHelper.GetUserDefaultAccountFromCartDetail(searchArgs[0], userId, cartId);

            var inventoryStockStatusList = InitializeInventoryStockStatus(searchArgs.Count);
            data = null;
            if (dicAvailableWarehouses != null && dicAvailableWarehouses.Count > 0)
            {
                var dicBTBPubStatus = new Dictionary<string, BTBPublisherStatus>();
                var dicETEInactiveFlag = new Dictionary<string, BTEInactiveFlag>();
                var dicStatusName = new Dictionary<OnhandInventoryStatus, string>();

                var warehousesAssigned = GetWareHousesAssigned(account, scMarketType, scCountryCode, scOrgId);

                int last30DaysDemand;
                var dataSet = GetDataSet(CatalogConnectionString, skusList, indexPositions, allWarehouses, account, out last30DaysDemand,
                    scMarketType, scCountryCode, scOrgId);
                data = dataSet;
                var rows = dataSet.Tables[0].Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var index = int.Parse(row[PropertyName.LineItem].ToString());
                    if (index >= 0)
                    {
                        var inventoryStockStatuses = inventoryStockStatusList[index] ?? new List<InventoryStockStatus>();
                        var inventoryStockStatus = new InventoryStockStatus();

                        var wareHouse = row[PropertyName.WarehouseId].ToString();
                        inventoryStockStatus.LineItem = index;
                        inventoryStockStatus.StockCondition = int.Parse(row[PropertyName.StockCondition].ToString());
                        inventoryStockStatus.InStockForRequest = int.Parse(row[PropertyName.InStockForRequest].ToString());
                        inventoryStockStatus.QuantityAvailable = int.Parse(row[PropertyName.InStockForRequest].ToString());
                        inventoryStockStatus.OnOrderQuantity = int.Parse(row[PropertyName.OnOrderQuantity].ToString());
                        inventoryStockStatus.WareHouseCode = wareHouse;

                        if (DetermineOnhandInventory(inventoryStockStatus, wareHouse, searchArgs[index].ProductType,
                                                     searchArgs[index].Flag, dicAvailableWarehouses, dicBTBPubStatus,
                                                     dicETEInactiveFlag, dicStatusName, warehousesAssigned,
                                                     searchArgs[index].PublishDate, searchArgs[index].ESupplier, searchArgs[index].ReportCode))
                        {
                            inventoryStockStatuses.Add(inventoryStockStatus);
                        }
                        inventoryStockStatusList[index] = inventoryStockStatuses;
                    }
                }
            }
            return inventoryStockStatusList;
        }

        private List<List<InventoryStockStatus>> InitializeInventoryStockStatus(int count)
        {
            var inventoryStockStatuses = new List<List<InventoryStockStatus>>();
            for (int i = 0; i < count; i++)
            {
                inventoryStockStatuses.Add(new List<InventoryStockStatus>());
            }
            return inventoryStockStatuses;
        }

        #endregion

       

 
    }
}
