using BT.TS360API.Common.CartFramework;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helper;
using BT.TS360API.Common.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using BT.TS360API.Common.Helpers;
using BT.TS360.NoSQL.Data.Common.Helper;
using ILSWinService.Helper;
using BT.TS360.NoSQL.Data.Common.Constants;

namespace ILSWinService
{
    public class ILSCartProcessor : BaseDAO
    {
        private int NumberOfILSThreads = 0;
        public Dictionary<ILSOrderRequest, bool> cartRequests = null;
        private static object lockProcess = new object();
        ThreadedLogger _threadedLogger;
        static BT.TS360.NoSQL.Data.Common.Helper.Emailer _emailer;
        ExceptionLogger _exceptionLogger;
        static string _emailTo;
        static string _currentEnvironment;
        public ILSCartProcessor()
        {
            cartRequests = new Dictionary<ILSOrderRequest, bool>();
            NumberOfILSThreads = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfILSThreads"]);
            _threadedLogger = new ThreadedLogger(AppSettings.LogFolder, AppSettings.LogFilePrefix);
            _emailer = new BT.TS360.NoSQL.Data.Common.Helper.Emailer(AppSettings.EmailSMTPServer);
            _exceptionLogger = new ExceptionLogger(AppSettings.ExceptionLoggingConnectionString);
            _emailTo = AppSettings.EmailTo;
            _currentEnvironment = AppSettings.CurrentEnvironment;
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["OrderDbConnString"]; }
        }

        public void ProcessCarts()
        {
            _threadedLogger.Write("Begin ProcessCarts", FileLoggingLevel.INFO);
            InitializeILSRequest();
            _threadedLogger.Write("End ProcessCarts ", FileLoggingLevel.INFO);

        }


        /// <summary>
        ///  Submit ILS Order to process
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void SubmitILSOrder(ILSOrderRequest request)
        {
            GlobalConfigurationHelper.Instance.Add(ILSConstant.ILSBaseAddress, ConfigurationManager.AppSettings[ILSConstant.ILSBaseAddress]);
            GlobalConfigurationHelper.Instance.Add(ILSConstant.ILSOrderValidatePath, ConfigurationManager.AppSettings[ILSConstant.ILSOrderValidatePath]);
            GlobalConfigurationHelper.Instance.Add(ILSConstant.ILSSubmitOrderPath, ConfigurationManager.AppSettings[ILSConstant.ILSSubmitOrderPath]);
            GlobalConfigurationHelper.Instance.Add(ILSConstant.ILSAuthorizePath, ConfigurationManager.AppSettings[ILSConstant.ILSAuthorizePath]);
            GlobalConfigurationHelper.Instance.Add(ILSConstant.ILSTokenPath, ConfigurationManager.AppSettings[ILSConstant.ILSTokenPath]);
            GlobalConfigurationHelper.Instance.Add(ILSConstant.ILSGetLogApiUrl, ConfigurationManager.AppSettings[ILSConstant.ILSGetLogApiUrl]);
            GlobalConfigurationHelper.Instance.Add(ILSConstant.ILSInsertLogApiUrl, ConfigurationManager.AppSettings[ILSConstant.ILSInsertLogApiUrl]);
            GlobalConfigurationHelper.Instance.Add(ILSConstant.ILSVendor, ConfigurationManager.AppSettings[ILSConstant.ILSVendor]);
            var val = OrdersDAOManager.Instance.ProcessILSOrder(request);
            lock (lockProcess)
            {
                cartRequests.Remove(request);
                ILSCartFile.Instance.DeleteCartFromILSFile(request.CartId);
            }

        }

        /// <summary>
        /// Get ILS Request data from database
        /// </summary>
        /// <returns></returns>
        private void InitializeILSRequest()
        {
            var pendingOrderCount = GetILSPendingOrderCount();
            _threadedLogger.Write("Pending Orders: " + pendingOrderCount, FileLoggingLevel.INFO);
            ILSOrderRequest ilsRequest = null;
            if (cartRequests.Where(xx => xx.Value == false).Count() < NumberOfILSThreads
                && pendingOrderCount > 0)
            {
                int lineItemThresholdLimit = Convert.ToInt32(ConfigurationManager.AppSettings["ILSOrderThresholdLimit"]);

                var conn = CreateSqlConnection();
                var command = CreateSqlSpCommand(ILSConstant.Proc_Get_ILS_Pending_Order, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    CartManager cartManager = new CartManager();
                    while (reader.Read())
                    {
                        ilsRequest = new ILSOrderRequest()
                        {
                            CartId = reader["BasketSummaryID"].ToString(),
                            UserId = reader["UpdatedBy"].ToString(),
                            MarcProfileId = reader["ILSMarcProfileId"].ToString(),
                            IlsVendorCode = reader["VendorCode"].ToString(),
                            OrderedDownloadedUserId = reader["OrderedDownloadedUserID"].ToString()
                        };

                        if (!cartRequests.Any(qry => qry.Key.CartId.Equals(ilsRequest.CartId)))
                        {
                            //Basket cartInfo = cartManager.GetBasketById(ilsRequest.CartId, ilsRequest.UserId);
                            var lineItemCount = DataAccessHelper.ConvertToInt(reader["TotalOrderLineCount"]);

                            if (lineItemCount > lineItemThresholdLimit)
                            {
                                cartRequests.Add(ilsRequest, false);
                            }
                        }

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    _threadedLogger.Write(ex.Message, FileLoggingLevel.ERROR);
                    _exceptionLogger.LogError(ex, "ILSWinService", "InitializeILSRequest");
                    _emailer.Send(_emailTo, string.Format("ILS Windows Service Error: " + _currentEnvironment + " [" + Environment.MachineName + "]"), ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            var inProcessCarts = cartRequests.Where(xx => xx.Value == true).ToList();
            var pendingIlsCarts = cartRequests.Where(qry => qry.Value == false).ToList();

            if (inProcessCarts.Count() < NumberOfILSThreads)
            {
                int pendingQueue = NumberOfILSThreads - inProcessCarts.Count();
                for (int ctr = 0; ctr < pendingIlsCarts.Count(); ctr++)
                {
                    if (ctr < pendingQueue)
                    {
                        ILSOrderRequest ilsOrderRequest = pendingIlsCarts.ElementAt(ctr).Key;
                        cartRequests[ilsOrderRequest] = true;
                        ILSCartFile.Instance.WriteILSFile(ilsOrderRequest);
                        Task.Factory.StartNew(() => SubmitILSOrder(ilsOrderRequest));
                        //SubmitILSOrder(ilsOrderRequest);
                    }
                    //else
                    //    break;
                }

            }

        }

        internal int GetILSPendingOrderCount()
        {
            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                using (var command = CreateSqlSpCommand(ILSConstant.Proc_Get_ILS_Pending_Order_Count, dbConnection))
                {
                    dbConnection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return DataAccessHelper.ConvertToInt(reader["ILSPendingOrderCount"]);
                        }
                    }

                    return 0;
                }
            }
        }
    }
}
