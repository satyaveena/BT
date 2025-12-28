using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using BT.TS360API.Common.BTPricingService;
using BT.TS360API.Common.Pricing;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360Constants;

namespace BT.TS360API.Common.DataAccess
{
    internal class PricingCalculatorDAO : BaseDAO
    {
        #region Constants

        internal const string ConnectStringProvider = "provider";
        internal const char Semicolon = ';';
        internal const string RealTimeSysInfo = "__realTimeSysInfo";
        internal const string DefaultTolassystemId = "NEXTGEN";
        internal const string DefaultTolaspricingPassword = "N@xtG3n";
        internal const int TolasServiceRetryTimes = 3;

        #endregion

        #region Singleton

        private static PricingCalculatorDAO _instance;
        public static PricingCalculatorDAO Instance
        {
            get { return _instance ?? (_instance = new PricingCalculatorDAO()); }
        }

        #endregion

        #region Implement BaseDAO

        /// <summary>
        /// DB Connection String
        /// </summary>
        public override string ConnectionString
        {
            get
            {
                //@"Password=P@ssword123;Persist Security Info=True;User ID=sa;Initial Catalog=Productcatalog1;Data Source=SHAREPOINT-SVR\RepositoryR2";
                var connectString = PricingConfiguration.Instance.ProductCatalogDbConnString;
                // Cut out Provider
                if (connectString.ToLower().Contains(ConnectStringProvider))
                {
                    int firstDelimeter = connectString.IndexOf(Semicolon);
                    connectString = connectString.Substring(firstDelimeter + 1);
                }

                return connectString;
            }
        }

        #endregion

        private int SqlCmdTimeout
        {
            get
            {
                var cmdTimeout = PricingConfiguration.Instance.SqlCmdTimeout;
                int iValue;
                if (string.IsNullOrEmpty(cmdTimeout) || !Int32.TryParse(cmdTimeout, out iValue))
                {
                    iValue = 300; //default
                }
                return iValue;
            }
        }
        /// <summary>
        /// Call to Product Catalog to get list price
        /// </summary>
        /// <param name="listPriceArguments"></param>
        /// <returns></returns>
        public List<ItemPricing> GetListPrice(IEnumerable<ListPricingArgument> listPriceArguments)
        {
            PricingLogger.LogDebug("PricingCalculatorDAO", "GetListPrice --> Begin");
            try
            {
                var ds = new DataSet();
                var dt = ConvertToListPricingArgumentTable(listPriceArguments);

                using (var conn = CreateSqlConnection())
                {
                    var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_GET_LIST_PRICE, conn);
                    var parameter = CreateTableParameter("@BTKeys", TableParameterType.ListPriceTable, dt);
                    cmd.Parameters.Add(parameter);
                    cmd.CommandTimeout = SqlCmdTimeout;

                    var da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

                var retListPrice = ConvertToListItemPricing(ds);
                return retListPrice;
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return null;
            }
            finally
            {
                PricingLogger.LogDebug("PricingCalculatorDAO", "GetListPrice <-- End");
            }

        }

        /// <summary>
        /// Calculate contract price from SOP
        /// </summary>
        /// <param name="sopPricingArguments"></param>
        /// <returns></returns>
        public List<ItemPricing> CalculateContractPrice(IEnumerable<SOPPricingArgument> sopPricingArguments, int? overriedQtyForDiscount)
        {
            PricingLogger.LogInfo("PricingCalculatorDAO", "CalculateContractPrice - SOP --> Begin");
            try
            {
                var ds = new DataSet();

                using (var conn = CreateSqlConnection())
                {
                    //4428: For Search Page Only and for Retail Customers Only we will pass a Static order quantity of 5 
                    //and a line quantity of 5 in the product search.  Applicable to both Basic Search and Advanced Search.  Hardcoded to Pricing.
                    //10297: apply 4428 for any market type
                    var discountArgs = sopPricingArguments.ToList();
                    if (overriedQtyForDiscount != null)
                    {
                        discountArgs = discountArgs.ConvertAll(x => new SOPPricingArgument(x));
                        discountArgs.ForEach(x => { x.OrderLineQuanity = overriedQtyForDiscount; x.TotalQuanity = overriedQtyForDiscount; });
                    }

                    var dt = ConvertToListSopPricingArgumentTable(discountArgs);
                    var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_GET_SOP_PRICING_DISCOUNTS, conn);
                    cmd.CommandTimeout = SqlCmdTimeout;
                    var parameter = CreateTableParameter("@utblprocPricingGetSOPPriceDiscounts",
                                                         TableParameterType.SopPriceDiscountTable, dt);
                    cmd.Parameters.Add(parameter);

                    var da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

                var itemPricings = ConvertToSOPPriceItemPricing(ds);
                if (itemPricings != null)
                {
                    PricingLogger.LogDebug("PricingCalculatorDAO", "==================ItemPricing after get SOP contract price:==================");
                    foreach (ItemPricing itemPricing in itemPricings)
                    {
                        var spa = sopPricingArguments.Where(x => x.BTKey == itemPricing.BTKey).FirstOrDefault();
                        if (spa != null)
                            itemPricing.ContractPrice = spa.ListPrice * (100 - itemPricing.ContractDiscountPercent) / 100 +
                                                        itemPricing.SurchargeAmount;// *spa.OrderLineQuanity;
                        PricingLogger.LogDebug("PricingCalculatorDAO", itemPricing.ToString());
                    }
                }

                return itemPricings;
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                var unCalculatedPrices = sopPricingArguments.Select(sopArg => new ItemPricing()
                {
                    ContractPrice = null,
                    BTKey = sopArg.BTKey
                }).ToList();
                return unCalculatedPrices;
            }
            finally
            {
                PricingLogger.LogInfo("PricingCalculatorDAO", "CalculateContractPrice  - SOP<-- End");
            }
        }

        /// <summary>
        /// Calculate contract price from TOLAS
        /// </summary>
        /// <param name="pricingArguments"></param>
        /// <returns></returns>        
        public List<ItemPricing> CalculateContractPrice(List<TOLASPricingArgument> pricingArguments)
        {
            PricingLogger.LogInfo("PricingCalculatorDAO", "CalculateContractPrice - TOLAS ==> Begin");
            var list = new List<ItemPricing>();
            //create pricing request
            var pricingRequest = CreateTolasPricingRequest(pricingArguments);

            if (pricingRequest != null)
            {
                var btPricingService = new BTPricingService.BTPricingService () { Url = PricingConfiguration.Instance.TolasServiceUrl };
                var count = 0;
                while (count < TolasServiceRetryTimes)
                {
                    try
                    {
                        //log tolas parameters
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in pricingRequest.ItemList)
                        {
                            sb.AppendFormat("[AcceptableDiscount:{0}, ItemId:{1}, PriceKey:{2}, ListPrice:{3}, ProductLine:{4}, Quantity:{5}, ReturnFlag:{6}]",
                                item.AcceptableDiscount, item.ItemId, item.PriceKey, item.ListPrice, item.ProductLine, item.Quantity, item.ReturnFlag);
                        }
                        var requestArg = string.Format("AccountID:{0}, SystemID:{1}, SystemPassword:{2}, WHSCOD:{3}, ItemList:{4}",
                            pricingRequest.AccountID, pricingRequest.SystemID, pricingRequest.SystemPassword, pricingRequest.WHSCOD, sb.ToString());
                        PricingLogger.LogInfo("PricingCalculatorDAO", string.Format("TOLAS PricingRequest:{0}", requestArg));
                        /////////////////////////////////
                        var pricingResponse = btPricingService.PricingRequestByClass(pricingRequest);

                        PricingLogger.LogInfo("PricingCalculatorDAO", string.Format("TOLAS Response Message:{0}",
                                                                  pricingResponse.StatusMessage));
                        if (pricingResponse.ItemsList.Length > 0)
                        {
                            var resItemList = pricingResponse.ItemsList;
                            foreach (var responseDetail in resItemList)
                            {
                                PricingLogger.LogDebug("PricingCalculatorDAO",
                                                       string.Format(
                                                           "ItemID:{0}, TOLAS Line Status Message:{1}, Discount Price:{2}, Discount Percent:{3} ",
                                                           responseDetail.ItemID, responseDetail.LineStatusMessage,
                                                           responseDetail.DiscountPrice, responseDetail.DiscountPercent));

                                var arg = pricingArguments.Find(x => x.ItemId == responseDetail.ItemID);
                                if (arg != null)
                                {
                                    var itemPricing = new ItemPricing
                                    {
                                        BTKey = arg.BTKey,
                                        BasketSummaryId = arg.BasketSummaryID,
                                        ContractPrice = responseDetail.DiscountPrice,
                                        ContractDiscountPercent = responseDetail.DiscountPercent
                                    };
                                    list.Add(itemPricing);
                                }
                            }
                        }
                        break;
                    }
                    catch (SoapException soapException)
                    {
                        Logger.LogException(soapException);
                        count++;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        break;
                    }
                }
            }
            PricingLogger.LogInfo("PricingCalculatorDAO", "CalculateContractPrice - TOLAS <== End");
            return list;
        }

        private PricingRequest CreateTolasPricingRequest(List<TOLASPricingArgument> pricingArguments)
        {
            var itemRequestDetails = new List<ItemRequestDetail>();
            string accountId = string.Empty, primaryWarehouse = string.Empty;
            if (pricingArguments.Count > 0)
            {
                accountId = pricingArguments[0].AccountERPNumber;
                primaryWarehouse = pricingArguments[0].PrimaryWarehouse;
            }

            foreach (TOLASPricingArgument pricingArgument in pricingArguments)
            {
                if (!string.IsNullOrEmpty(pricingArgument.AccountERPNumber))
                {
                    //make sure account id and WH not missing
                    accountId = pricingArgument.AccountERPNumber;
                    primaryWarehouse = pricingArgument.PrimaryWarehouse;

                    var itemRequestDetail = new ItemRequestDetail
                    {
                        AcceptableDiscount = pricingArgument.AcceptableDiscount,
                        ItemId = pricingArgument.ItemId,
                        PriceKey = pricingArgument.PriceKey,
                        ListPrice = pricingArgument.ListPrice.HasValue ? pricingArgument.ListPrice.Value : 0,
                        ProductLine = pricingArgument.ProductLine,
                        Quantity = pricingArgument.TotalLineQuanity,
                        ReturnFlag = pricingArgument.ReturnFlag.ToString().ToLower() == "false" ? "N" : "1"
                    };
                    itemRequestDetails.Add(itemRequestDetail);
                }
            }

            if (itemRequestDetails.Count > 0)
            {
                var sysInfo = PricingConfiguration.Instance.RealTimeWsInfo; //GetRealTimeWSInfo();
                var pricingRequest = new PricingRequest
                {
                    AccountID = accountId,
                    ItemList = itemRequestDetails.ToArray(),
                    SystemID = sysInfo != null ? sysInfo[0] : DefaultTolassystemId,
                    SystemPassword = sysInfo != null ? sysInfo[1] : DefaultTolaspricingPassword,
                    WHSCOD = primaryWarehouse
                };
                return pricingRequest;
            }
            return null;
        }

        #region Helpers

        private DataTable CreateListPricingArgumentTable()
        {
            var obj = new ListPricingArgument { BTKey = string.Empty, eMarket = string.Empty, eTier = string.Empty };
            var dt = new DataTable("ListPricingArgument");
            dt.Columns.Add("BTKey", obj.BTKey.GetType());
            dt.Columns.Add("eMarket", obj.eMarket.GetType());
            dt.Columns.Add("eTier", obj.eTier.GetType());
            return dt;
        }

        private DataTable CreateListSOPPricingArgumentTable()
        {
            var obj = new SOPPricingArgument
            {
                BTKey = string.Empty,
                PlanID = string.Empty,
                PriceKey = string.Empty,
                TotalQuanity = 0,
                OrderLineQuanity = 0
            };
            var dt = new DataTable("utblprocPricingGetSOPPriceDiscounts");
            dt.Columns.Add("BTKey", obj.BTKey.GetType());
            dt.Columns.Add("PlanID", obj.PlanID.GetType());
            dt.Columns.Add("PriceKey", obj.PriceKey.GetType());
            dt.Columns.Add("TotalQuanity", obj.TotalQuanity.HasValue ? obj.TotalQuanity.GetType() : typeof(int));
            dt.Columns.Add("OrderLineQuanity", obj.OrderLineQuanity.HasValue ? obj.OrderLineQuanity.GetType() : typeof(int));
            return dt;
        }

        private DataTable ConvertToListSopPricingArgumentTable(IEnumerable<SOPPricingArgument> sopPricingArguments)
        {
            var dt = CreateListSOPPricingArgumentTable();
            if (sopPricingArguments != null)
            {
                var list = sopPricingArguments.ToList();
                if (list.Any())
                    list.ForEach(r => dt.Rows.Add(r.BTKey,
                                                  r.PlanID,
                                                  r.PriceKey,
                                                  r.TotalQuanity,
                                                  r.OrderLineQuanity));
            }
            return dt;
        }

        private DataTable ConvertToListPricingArgumentTable(IEnumerable<ListPricingArgument> listPriceArguments)
        {
            var dt = CreateListPricingArgumentTable();
            if (listPriceArguments != null)
            {
                var list = listPriceArguments.ToList();
                if (list.Any())
                    list.ForEach(r => dt.Rows.Add(r.BTKey, r.eMarket, r.eTier));
            }
            return dt;
        }

        private List<ItemPricing> ConvertToListItemPricing(DataSet ds)
        {
            List<ItemPricing> list = null;
            if (ds != null && ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0)
            {
                list = (from DataRow dataRow in ds.Tables[0].Rows
                        select new ItemPricing
                        {
                            BTKey = BaseDAO.ConvertToString(dataRow["BTKey"]),
                            ListPrice = BaseDAO.ConvertTodecimal(dataRow["ListPrice"])
                        }).ToList();

            }
            return list;
        }

        private List<ItemPricing> ConvertToSOPPriceItemPricing(DataSet ds)
        {
            List<ItemPricing> list = null;
            if (ds != null && ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0)
            {
                list = (from DataRow dataRow in ds.Tables[0].Rows
                        select new ItemPricing
                        {
                            BTKey = BaseDAO.ConvertToString(dataRow["BTKey"]),
                            ContractDiscountPercent = BaseDAO.ConvertTodecimal(dataRow["percentDiscount"]),
                            SurchargeAmount = BaseDAO.ConvertTodecimal(dataRow["surchargeAmount"])
                        }).ToList();

            }
            return list;
        }

        private SqlParameter CreateTableParameter(string parameterName, string parameterTypeName, DataTable value)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = SqlDbType.Structured,
                TypeName = parameterTypeName,
                Value = value
            };
        }

        #endregion
    }
}
