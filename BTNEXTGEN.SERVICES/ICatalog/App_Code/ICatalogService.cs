using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BTNextGen.Services.Common;
using System.Collections.ObjectModel;

namespace BTNextGen.Services.ICatalogs
{

    [ServiceContract]
    public interface ICatalogService
    {

        //[OperationContract]
        //string CreateNewProducts(BTProductCollection newProducts);

        //[OperationContract]
        //string UpdateProducts(BTProductCollection updateProducts);

        [OperationContract]
        CategoryRespCollection GetCategoryDetails(string catalogName);



    }

}
