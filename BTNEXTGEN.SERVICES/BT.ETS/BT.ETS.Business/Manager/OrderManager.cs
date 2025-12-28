using BT.ETS.Business.DAO;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Manager.Interface;
using BT.ETS.Business.Models;
using BT.ETS.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.ServiceContracts;
using BT.ETS.Business.Constants;
using MongoDB.Bson;
using BT.TS360Constants;

using etsModels = BT.ETS.Business.Models;

namespace BT.ETS.Business.Manager
{
    public class OrderManager
    {

        #region Private Member

        private static volatile OrderManager _instance;
        private static readonly object SyncRoot = new Object();
        public static OrderManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderManager();
                }

                return _instance;
            }
        }

        #endregion

        #region Public Method
        public async Task<InsertedCartResult> InsertEtsCart(string espLibraryId, string etsCartId, string cartName, string cartNote, string userId, List<LineItemInput> lines)
        {
            var insertedResult = await OrderDAO.Instance.InsertEtsCart(espLibraryId, etsCartId, cartName, cartNote, userId, lines);
            return HandleEtsCartInsert(insertedResult);
        }

        public async Task<DupCheckDataResult> GetDupChecks(string userId, List<string> btKeyList, string dupCheckC, string dupCheckDownloadCart, string dupCheckH)
        {
            //TFS36395 detect btkey that has more than 10.
            List<string> validBtkeyList = new List<string>();
            List<string> errorBtkeyList = new List<string>();
            
            foreach (var btkey in btKeyList)
            {
                if (!string.IsNullOrEmpty(btkey) && btkey.Length == 10)
                    validBtkeyList.Add(btkey);
                else
                    errorBtkeyList.Add(btkey);
            }

            if (validBtkeyList.Count == 0)
                return await CreateDupCheckResultWithErrorItemsOnly(errorBtkeyList);

            var result = new DupCheckDataResult();

            int MaxDupCheckBatchSize_CH = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MaxDupeCheckBatchSize_CH);

            if (validBtkeyList.Count <= MaxDupCheckBatchSize_CH)
            {
                result = await OrderDAO.Instance.GetCheckDups(userId, validBtkeyList, dupCheckC, dupCheckDownloadCart, dupCheckH);
            }
            else
            {
                double loopCount = Math.Ceiling((double)validBtkeyList.Count / MaxDupCheckBatchSize_CH);

                for (int i = 0; i < loopCount; i++)
                {
                    var tempResult = await OrderDAO.Instance.GetCheckDups(userId, validBtkeyList.Skip(i * MaxDupCheckBatchSize_CH).Take(MaxDupCheckBatchSize_CH).ToList(), dupCheckC, dupCheckDownloadCart, dupCheckH);
                    if (i == 0)
                        result = tempResult;
                    else
                        result.Data.Merge(tempResult.Data);
                }
            }           
            
            return HandleDupCheckResult(result, errorBtkeyList);
            
        }

        public async Task<etsModels.OrdersDupCheckResponse> GetOrderServiceDuplicates(etsModels.OrdersDupCheckRequest request)
        {
            var response = new etsModels.OrdersDupCheckResponse();
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.UserId) && request.BTKeys != null)
                {

                    if (string.Equals(request.OrderCheckType, "myaccounts", StringComparison.OrdinalIgnoreCase))
                    {
                        request.UserAccounts = await ProfileManager.Instance.GetUserAccounts(request.UserId);
                    }

                    var dicOrderDuplicates = await GetOrderDuplicates(request);

                    if (dicOrderDuplicates != null)
                    {
                        var dupItems = new List<etsModels.DuplicateItem>();
                        foreach (var item in dicOrderDuplicates)
                        {
                            dupItems.Add(new etsModels.DuplicateItem { BTKey = item.Key, IsDuplicated = item.Value });
                        }

                        response.DuplicateItems = dupItems;
                    }
                }
            }

            return response;
        }
        
        public async Task<Dictionary<string, bool>> GetOrderDuplicates(etsModels.OrdersDupCheckRequest request)
        {
            var results = new Dictionary<string, bool>();

            if (request != null && request.BTKeys != null && request.BTKeys.Count > 0)
            {               
                // init results
                foreach (var btKey in request.BTKeys)
                {
                    if (btKey != null)
                         results.Add(btKey, false);                        
                }

                // get Dup O from MongoDB
                int MaxDupCheckBatchSize_O = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MaxDupeCheckBatchSize_O);
                var bsonDocuments = new List<BsonDocument>();
                if (request.BTKeys.Count <= MaxDupCheckBatchSize_O)
                {
                    bsonDocuments = await CommonDAO.Instance.GetOrderDuplicates(request);
                }
                else
                {
                    double loopCount = Math.Ceiling((double)request.BTKeys.Count / MaxDupCheckBatchSize_O);
                    var tempReqBTKeys = request.BTKeys;
                    for (int i = 0; i < loopCount; i++)
                    {
                        request.BTKeys = tempReqBTKeys.Skip(i * MaxDupCheckBatchSize_O).Take(MaxDupCheckBatchSize_O).ToList();
                        bsonDocuments.AddRange(await CommonDAO.Instance.GetOrderDuplicates(request));
                    }
                }

                if (bsonDocuments != null)
                {
                    foreach (var document in bsonDocuments)
                    {
                        var docBTKey = document["BTKey"].AsString;

                        if (results.ContainsKey(docBTKey))
                            results[docBTKey] = true;
                    }
                }
            }

            return results;
        }

        private async Task<DupCheckDataResult> CreateDupCheckResultWithErrorItemsOnly(List<string> inputBtKeys)
        {
            var result = new DupCheckDataResult();
            var dupCheckResult = new DupCheckResult();
            result.DupCheckResult = dupCheckResult;
            dupCheckResult.ErrorItems = new List<ErrorItem>();
            dupCheckResult.Items = new List<ProductDupCheckStatus>();//TFS36707
            foreach (var btkey in inputBtKeys)
            {
                var line = new ErrorItem();
                line.BTKey = btkey;
                line.Message = BusinessExceptionConstants.INVALID_ITEM;

                dupCheckResult.ErrorItems.Add(line);
            }
            return result;
        }

        public async Task<List<DupCheckCartInfo>> GetCheckDupDetails(string userId, string btKey, string dupCheckStatusType, string dupCheckPreference, string dupCheckDownloadCartType)
        {
            var result = await OrderDAO.Instance.GetCheckDupDetails(userId, btKey, dupCheckStatusType, dupCheckPreference, dupCheckDownloadCartType);
            return HandleDupCheckDetailResult(result, btKey, dupCheckStatusType);
        }

        public async Task<bool> ValidateUserAndOrg(string userId, string orgId)
        {
            return await OrderDAO.Instance.ValidateUserAndOrg(userId, orgId);
        }

        public async Task<GridTemplateResult> GetGridTemplatesByUserId(string userId)
        {
            var result = await OrderDAO.Instance.GetGridTemplatesByUserId(userId);
            return HandleGridTemplateResult(result);
        }

        public async Task<PricingRequests> GetProductPricingPreferences(string userId, List<string> btKeyList)
        {
            var result = await OrderDAO.Instance.GetProductPricingPreferences(userId, btKeyList);
            if (result == null || result.Tables == null || result.Tables.Count == 0)
            {
                var msg = string.Format("GetProductPricingPreferences: dsInput is NULL or empty. First result-set is required. UserId='{0}'. BtKeys = '{1}'"
                    , userId, string.Join(",", btKeyList));
                throw new Exception(msg);
            }

            PricingRequests pr = new PricingRequests();
            pr.SearchRequest = ConvertToSearchRequest(userId, result.Tables[0]);
           
            if (result.Tables.Count > 1)
                pr.ProductVasList = RetrieveProductVas(result.Tables[1]);

            if (result.Tables.Count > 2)
                pr.ErrorItems = RetrieveErrorItems(result.Tables[2]);
            return pr;
        }
        #endregion

        #region Private Method

        private SearchRequest ConvertToSearchRequest(string userId, DataTable dtInput)
        {


            var dr = dtInput.Rows[0];

            SearchRequest request = new SearchRequest();
            request.UserId = userId;
            request.IsHideNetPriceDiscountPercentage = false;
            request.ShowExpectedDiscountPriceForSearch = true;
            request.MarketTypeString = CommonHelper.SqlDataConvertTo<string>(dr, "MarketType");
            
            request.AccountPricingData = new AccountInfoForPricing();
            request.AccountPricingData.EnableProcessingCharges = CommonHelper.SqlDataConvertTo<bool>(dr, "EnableProcessingCharges");
            request.AccountPricingData.BookProcessingCharge = CommonHelper.SqlDataConvertTo<decimal>(dr, "BookProcessingCharge");
            request.AccountPricingData.PaperbackProcessingCharge = CommonHelper.SqlDataConvertTo<decimal>(dr, "PaperbackProcessingCharge");
            request.AccountPricingData.SpokenWordProcessingCharge = CommonHelper.SqlDataConvertTo<decimal>(dr, "SpokenWordProcessingCharge");

            //ETS sprint 51 pricing for Book and eBook only
            //request.AccountPricingData.MovieProcessingCharge = CommonHelper.SqlDataConvertTo<decimal>(dr, "MovieProcessingCharge");
            //request.AccountPricingData.MusicProcessingCharge = CommonHelper.SqlDataConvertTo<decimal>(dr, "MusicProcessingCharge");
            request.AccountPricingData.SalesTax = (float)CommonHelper.SqlDataConvertTo<decimal>(dr, "SalesTax");
            

            request.SearchData = new SearchByIdData();
            request.SearchData.CountryCode = CommonHelper.SqlDataConvertTo<string>(dr, "CountryCode");
            request.SearchData.ESuppliers = CommonHelper.ConvertToStringArrayWithoutFirstNumberElement(CommonHelper.SqlDataConvertTo<string>(dr, "ESuppliers"));
            request.SearchData.SimonSchusterEnabled = CommonHelper.SqlDataConvertTo<bool>(dr, "SimonSchusterEnabled");

            request.Targeting = new TargetingValues();
            request.Targeting.AudienceType = CommonHelper.ConvertToStringArrayWithoutFirstNumberElement(CommonHelper.SqlDataConvertTo<string>(dr, "AudienceType"));

            int marketTypeInt;
            if (int.TryParse(request.MarketTypeString, out marketTypeInt))
                request.Targeting.MarketType = (BT.TS360Constants.MarketType)marketTypeInt;

            request.Targeting.OrganizationName = CommonHelper.SqlDataConvertTo<string>(dr, "OrganizationName");
            request.Targeting.OrgId = CommonHelper.SqlDataConvertTo<string>(dr, "OrgId");
            request.Targeting.PIGName = CommonHelper.SqlDataConvertTo<string>(dr, "PIGName");
            request.Targeting.ProductType = CommonHelper.ConvertToStringArrayWithoutFirstNumberElement(CommonHelper.SqlDataConvertTo<string>(dr, "ProductType"));
            request.Targeting.SiteBranding = CommonHelper.SqlDataConvertTo<string>(dr, "SiteBranding");

            request.IsFromExternalAPI = true;

            return request;
        }
        private Dictionary<string, ProductPricing> RetrieveProductVas(DataTable dtInput)
        {
            Dictionary<string, ProductPricing> products = new Dictionary<string, ProductPricing>();
            if (dtInput.Rows == null || dtInput.Rows.Count == 0)
                return products;

            foreach (DataRow dr in dtInput.Rows)
            {
                ProductPricing pp = new ProductPricing();
                pp.BookDigitalProcessingCharge = Math.Round(CommonHelper.SqlDataConvertTo<decimal>(dr, "BookDigitalProcessingCharge"), 2);
                pp.AdditionalPaperbackCharge = Math.Round(CommonHelper.SqlDataConvertTo<decimal>(dr, "PaperbackProcessingCharge"), 2);
                pp.SpokenWordCharge = Math.Round(CommonHelper.SqlDataConvertTo<decimal>(dr, "SpokenWordProcessingCharge"), 2);
                pp.SalesTaxPercentage = Math.Round(CommonHelper.SqlDataConvertTo<decimal>(dr, "SalesTax"), 2);
                pp.BTKey = CommonHelper.SqlDataConvertTo<string>(dr, "BTKey");

                if (!products.ContainsKey(pp.BTKey))
                    products.Add(pp.BTKey, pp);
            }
            return products;
        }

        private List<ErrorItem> RetrieveErrorItems(DataTable dtInput)
        {
            List<ErrorItem> errorItems = new List<ErrorItem>();
            foreach (DataRow row in dtInput.Rows)
            {
                var line = new ErrorItem();
                line.BTKey = CommonHelper.SqlDataConvertTo<string>(row, "BTKey");
                line.Message = CommonHelper.SqlDataConvertTo<string>(row, "FailureMessage");

                errorItems.Add(line);
            }

            return errorItems;
        }
        private List<DupCheckCartInfo> HandleDupCheckDetailResult(DataSet dsInput, string btKey, string dupCheckStatusType)
        {
            List<DupCheckCartInfo> dupCheckData = new List<DupCheckCartInfo>();
            if (dsInput == null || dsInput.Tables == null || dsInput.Tables.Count == 0
               || dsInput.Tables[0].Rows == null || dsInput.Tables[0].Rows.Count == 0)
                throw new BusinessException(202);

            foreach (DataRow row in dsInput.Tables[0].Rows)
            {
                var cart = new DupCheckCartInfo();
                cart.CartId = CommonHelper.SqlDataConvertTo<string>(row, "CartID");
                cart.CartName = CommonHelper.SqlDataConvertTo<string>(row, "CartName");
                cart.CartStatus = CommonHelper.SqlDataConvertTo<string>(row, "CartStatus");
                cart.CartLastUpdatedDateTime = CommonHelper.SqlDataConvertTo<DateTime>(row, "CartLastUpdatedDateTime");
                cart.Quantity = CommonHelper.SqlDataConvertTo<int>(row, "Quantity");
                cart.BasketOwnerId = CommonHelper.SqlDataConvertTo<string>(row, "BasketOwnerID");
                cart.PurchaseOrderNumber = CommonHelper.SqlDataConvertTo<string>(row, "PurchaseOrderNumber");
                cart.AccountNumber = CommonHelper.SqlDataConvertTo<string>(row, "AccountID");

                if (dsInput.Tables.Count > 1)
                {
                    try
                    {
                        cart.GridLines = new List<GridLineInfor>();
                        foreach (DataRow row1 in dsInput.Tables[1].Rows)
                        {
                            var gridCartId = CommonHelper.SqlDataConvertTo<string>(row1, "CartID");
                            if (cart.CartId == gridCartId)
                            {
                                var grid = new GridLineInfor();
                                grid.Quantity = CommonHelper.SqlDataConvertTo<int>(row1, "Quantity");
                                grid.CollectionCode = CommonHelper.SqlDataConvertTo<string>(row1, "CollectionCode");
                                grid.AgencyCode = CommonHelper.SqlDataConvertTo<string>(row1, "AgencyCode");
                                //38050 : Adding ItemTypeCode and callNumber in response
                                grid.ItemTypeCode = CommonHelper.SqlDataConvertTo<string>(row1, "ItemTypeCode");
                                grid.CallNumberText = CommonHelper.SqlDataConvertTo<string>(row1, "CallNumberText");
                                grid.GridFieldName1 = CommonHelper.SqlDataConvertTo<string>(row1, "GridFieldName1");
                                grid.GridCode1 = CommonHelper.SqlDataConvertTo<string>(row1, "GridCode1");
                                grid.GridFieldName2 = CommonHelper.SqlDataConvertTo<string>(row1, "GridFieldName2");
                                grid.GridCode2 = CommonHelper.SqlDataConvertTo<string>(row1, "GridCode2");
                                grid.GridFieldName3 = CommonHelper.SqlDataConvertTo<string>(row1, "GridFieldName3");
                                grid.GridCode3 = CommonHelper.SqlDataConvertTo<string>(row1, "GridCode3");
                                grid.GridFieldName4 = CommonHelper.SqlDataConvertTo<string>(row1, "GridFieldName4");
                                grid.GridCode4 = CommonHelper.SqlDataConvertTo<string>(row1, "GridCode4");
                                grid.GridFieldName5 = CommonHelper.SqlDataConvertTo<string>(row1, "GridFieldName5");
                                grid.GridCode5 = CommonHelper.SqlDataConvertTo<string>(row1, "GridCode5");
                                grid.GridFieldName6 = CommonHelper.SqlDataConvertTo<string>(row1, "GridFieldName6");
                                grid.GridCode6 = CommonHelper.SqlDataConvertTo<string>(row1, "GridCode6");

                                cart.GridLines.Add(grid);
                            }
                        }
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(string.Format("Load data from DataTable #2 for 'GridLines' data of CartId:'{0}'. Error message: {1}", cart.CartId, ex2.Message), ex2);
                    }
                }

                if (dsInput.Tables.Count > 2)
                {
                    try
                    {
                        cart.Orders = new List<OrderInfor>();
                        foreach (DataRow row2 in dsInput.Tables[2].Rows)
                        {
                            var orderCartId = CommonHelper.SqlDataConvertTo<string>(row2, "CartID");
                            if (cart.CartId == orderCartId)
                            {
                                var order = new OrderInfor();
                                order.OrderNumber = CommonHelper.SqlDataConvertTo<string>(row2, "OrderNumber");
                                order.Warehouse = CommonHelper.SqlDataConvertTo<string>(row2, "Warehouse");
                                order.Shipped = CommonHelper.SqlDataConvertTo<int>(row2, "Shipped");
                                order.InProcess = CommonHelper.SqlDataConvertTo<int>(row2, "InProcess");
                                order.Cancelled = CommonHelper.SqlDataConvertTo<int>(row2, "Cancelled");
                                order.Backordered = CommonHelper.SqlDataConvertTo<int>(row2, "Backordered");
                                order.Reserved = CommonHelper.SqlDataConvertTo<int>(row2, "Reserved");

                                cart.Orders.Add(order);
                            }
                        }
                    }
                    catch (Exception ex3)
                    {
                        throw new Exception(string.Format("Load data from DataTable #3 for 'Orders' data of CartId:'{0}'. Error message: {1}", cart.CartId, ex3.Message, ex3));
                    }
                }

                dupCheckData.Add(cart);
            }

            return dupCheckData;

        }
        private DupCheckDataResult HandleDupCheckResult(DupCheckDataResult dsInput, List<string> errorBtKeyList)
        {
            if (dsInput == null || dsInput.Data == null || dsInput.Data.Tables == null || dsInput.Data.Tables.Count == 0
                || dsInput.Data.Tables[0].Rows == null || dsInput.Data.Tables[0].Rows.Count == 0)
                throw new BusinessException(202);

            var dupCheckResult = new DupCheckResult();
            dupCheckResult.Items = new List<ProductDupCheckStatus>();
            foreach (DataRow row in dsInput.Data.Tables[0].Rows)
            {
                var checkDupResult = "";
                var checkDup = CommonHelper.SqlDataConvertTo<bool>(row, "CartDup");
                checkDupResult += checkDup ? "C," : "";

                //checkDup = CommonHelper.SqlDataConvertTo<bool>(row, "OrderDup");
                //checkDupResult += checkDup ? "O," : "";

                checkDup = CommonHelper.SqlDataConvertTo<bool>(row, "HoldingsDup");
                checkDupResult += checkDup ? "H," : "";


                var item = new ProductDupCheckStatus();
                item.BTKey = CommonHelper.SqlDataConvertTo<string>(row, "BTKey");
                item.DupCheckStatus = checkDupResult.TrimEnd(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                dupCheckResult.Items.Add(item);
            }

            if (dsInput.Data.Tables.Count > 1)
            {
                dupCheckResult.ErrorItems = new List<ErrorItem>();
                foreach (DataRow row in dsInput.Data.Tables[1].Rows)
                {
                    var line = new ErrorItem();
                    line.BTKey = CommonHelper.SqlDataConvertTo<string>(row, "BTKey");
                    line.Message = CommonHelper.SqlDataConvertTo<string>(row, "FailureMessage");

                    dupCheckResult.ErrorItems.Add(line);
                }
            }

            if (errorBtKeyList.Count > 0)
            {
                if (dupCheckResult.ErrorItems == null)
                    dupCheckResult.ErrorItems = new List<ErrorItem>();
                foreach (var btkey in errorBtKeyList)
                {
                    var line = new ErrorItem();
                    line.BTKey = btkey;
                    line.Message = BusinessExceptionConstants.INVALID_ITEM;

                    dupCheckResult.ErrorItems.Add(line);
                }
            }
            dsInput.DupCheckResult = dupCheckResult;
            return dsInput;
        }
        private InsertedCartResult HandleEtsCartInsert(DataSet dsInput)
        {
            if (dsInput == null || dsInput.Tables == null || dsInput.Tables.Count == 0)
                throw new Exception("HandleEtsCartInsert returns NULL data.");

            var insertedCart = new InsertedCartResult();
            insertedCart.Carts = new List<CartResult>();
            foreach (DataRow row in dsInput.Tables[0].Rows)
            {
                var cart = new CartResult();
                cart.CartName = CommonHelper.SqlDataConvertTo<string>(row, "CartName");
                cart.CartUrl = CommonHelper.SqlDataConvertTo<string>(row, "CartURL");

                if (cart.CartName != null && cart.CartUrl != null)
                {
                    // get CartID from CartUrl
                    cart.CartId = CommonHelper.ExtractUrlQueryString(cart.CartUrl, "cartId");

                    insertedCart.Carts.Add(cart);
                }
            }

            if (dsInput.Tables.Count > 1)
            {
                insertedCart.ErrorItems = new List<ErrorItem>();
                foreach (DataRow row in dsInput.Tables[1].Rows)
                {
                    var line = new ErrorItem();
                    line.BTKey = CommonHelper.SqlDataConvertTo<string>(row, "BTKey");
                    line.Message = CommonHelper.SqlDataConvertTo<string>(row, "FailureMessage");

                    insertedCart.ErrorItems.Add(line);
                }
            }

            return insertedCart;
        }
        private GridTemplateResult HandleGridTemplateResult(DataSet dsInput)
        {
            if (dsInput == null || dsInput.Tables == null || dsInput.Tables.Count == 0
               || dsInput.Tables[0].Rows == null || dsInput.Tables[0].Rows.Count == 0)
                throw new Exception("HandleGridTemplateResult: dsInput is NULL or empty.");

            var gridTemplateResult = new GridTemplateResult();
            gridTemplateResult.OrganizationId = CommonHelper.SqlDataConvertTo<string>(dsInput.Tables[0].Rows[0], "OrganiziatonId");
            gridTemplateResult.ESPBranchField = CommonHelper.SqlDataConvertTo<string>(dsInput.Tables[0].Rows[0], "ESPBranchField");
            gridTemplateResult.ESPFundField = CommonHelper.SqlDataConvertTo<string>(dsInput.Tables[0].Rows[0], "ESPFundField");

            if (dsInput.Tables.Count > 1)
            {
                try
                {
                    gridTemplateResult.GridTemplates = new List<GridTemplate>();
                    foreach (DataRow row1 in dsInput.Tables[1].Rows)
                    {
                        var gridTemplate = new GridTemplate();
                        gridTemplate.GridTemplateId = CommonHelper.SqlDataConvertTo<string>(row1, "GridTemplateId");
                        gridTemplate.GridTemplateName = CommonHelper.SqlDataConvertTo<string>(row1, "GridTemplateName");
                        gridTemplate.GridTemplateDesc = CommonHelper.SqlDataConvertTo<string>(row1, "GridTemplateDesc");
                        gridTemplateResult.GridTemplates.Add(gridTemplate);
                    }
                }
                catch (Exception ex2)
                {
                    throw new Exception(string.Format("Load data from DataTable #2. Error message: {0}", ex2.Message), ex2);
                }
            }

            if (dsInput.Tables.Count > 2 && gridTemplateResult.GridTemplates != null)
            {

                foreach (var gridTemplate in gridTemplateResult.GridTemplates)
                {
                    try
                    {
                        gridTemplate.GridTemplateLines = new List<GridTemplateLine>();
                        for (var i = dsInput.Tables[2].Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow row1 = dsInput.Tables[2].Rows[i];
                            var parentTemplateId = CommonHelper.SqlDataConvertTo<string>(row1, "GridTemplateId");
                            if (gridTemplate.GridTemplateId == parentTemplateId)
                            {
                                var line = new GridTemplateLine();
                                line.GridTemplateLineId = CommonHelper.SqlDataConvertTo<string>(row1, "GridTemplateLineID");
                                line.Quantity = CommonHelper.SqlDataConvertTo<int>(row1, "Quantity");
                                line.AgencyId = CommonHelper.SqlDataConvertTo<string>(row1, "AgencyId");
                                line.AgencyCode = CommonHelper.SqlDataConvertTo<string>(row1, "AgencyCode");
                                line.AgencyDesc = CommonHelper.SqlDataConvertTo<string>(row1, "AgencyDesc");

                                line.ItemTypeId = CommonHelper.SqlDataConvertTo<string>(row1, "ItemTypeId");
                                line.ItemTypeCode = CommonHelper.SqlDataConvertTo<string>(row1, "ItemTypeCode");
                                line.ItemTypeDesc = CommonHelper.SqlDataConvertTo<string>(row1, "ItemTypeDesc");

                                line.CollectionId = CommonHelper.SqlDataConvertTo<string>(row1, "CollectionId");
                                line.CollectionCode = CommonHelper.SqlDataConvertTo<string>(row1, "CollectionCode");
                                line.CollectionDesc = CommonHelper.SqlDataConvertTo<string>(row1, "CollectionDesc");

                                line.CallNumberText = CommonHelper.SqlDataConvertTo<string>(row1, "CallNumberText");

                                line.UserCode1Id = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode1Id");
                                line.UserCode1Code = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode1Code");
                                line.UserCode1Desc = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode1Desc");

                                line.UserCode2Id = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode2Id");
                                line.UserCode2Code = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode2Code");
                                line.UserCode2Desc = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode2Desc");

                                line.UserCode3Id = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode3Id");
                                line.UserCode3Code = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode3Code");
                                line.UserCode3Desc = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode3Desc");

                                line.UserCode4Id = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode4Id");
                                line.UserCode4Code = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode4Code");
                                line.UserCode4Desc = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode4Desc");

                                line.UserCode5Id = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode5Id");
                                line.UserCode5Code = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode5Code");
                                line.UserCode5Desc = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode5Desc");

                                line.UserCode6Id = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode6Id");
                                line.UserCode6Code = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode6Code");
                                line.UserCode6Desc = CommonHelper.SqlDataConvertTo<string>(row1, "UserCode6Desc");

                                gridTemplate.GridTemplateLines.Add(line);
                                dsInput.Tables[2].Rows.Remove(row1);
                            }
                        }//end for gridtemplatelines
                    }
                    catch (Exception ex3)
                    {
                        throw new Exception(string.Format("Load data from DataTable #3 for 'gridTemplateId'='{0}'. Error message: {1}", gridTemplate.GridTemplateId, ex3.Message, ex3));
                    }
                } // end for gridtemplates

            }



            return gridTemplateResult;

        }

        #endregion
    }
}
