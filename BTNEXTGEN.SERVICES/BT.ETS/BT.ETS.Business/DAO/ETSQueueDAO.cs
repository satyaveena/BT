using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.DAO
{
    public class ETSQueueDAO
    {
        #region Public Property
        private string mongoConnectionString = ConfigurationManager.AppSettings["MongoDBConnectionString"].ToString();
        #endregion

        #region Public Methods

        #endregion
    }
}
