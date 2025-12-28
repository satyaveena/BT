using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;

namespace BTNextGen.Services.IOrders
{

    [ServiceContract]
    public interface IOrdersService
    {


        [OperationContract]
        BasketDetailsResponse CreateLegacyBasket(BasketDetails basketList);

        [OperationContract]
        BasketDetailsResponseCollection CreateLegacyBaskets(BasketDetailsCollection basketList);

        [OperationContract]
        string GetCyberSourceReport(CyberSourceReportRequest reportRequest);

        // TODO: Add purge basket service operation here
        [OperationContract]
        DataSet GetAgedBaskets(string purgeBasketThresholdInDays, string maxBatchSize);

        [OperationContract]
        OrderDetailResponseCollection GetOrderDetails ( string accountNum);
    }



    

}
