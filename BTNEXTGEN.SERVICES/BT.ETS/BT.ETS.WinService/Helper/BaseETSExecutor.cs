using BT.ETS.API.Services;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using BT.ETS.Business.MongDBLogger.ELMAHLogger;
using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BT.ETS.WinService.Helper
{
    public abstract class BaseETSExecutor
    {
        protected OrderServices _orderServices;
        protected CommonHelper _ETSCommonHelper;
        protected Dictionary<string, string> APIKey;

        private ErrorLog _logger;

        protected ErrorLog logger
        {
            get
            {
                if (_logger == null)
                    _logger = new ELMAHMongoLogger();
                return _logger;
            }
        }

        public BaseETSExecutor()
        {
            _orderServices = new OrderServices();

            _ETSCommonHelper = new CommonHelper();
            APIKey = new Dictionary<string, string>
                    {
                        { "X-API-KEY", AppSettings.ESPVendorKey }
                    };
        }

        public abstract string MongoResponseFieldName { get; }
    }
}
