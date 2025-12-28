using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BTNextGen.Services.IPromotions
{
    /// <summary>
    /// Contracts
    /// </summary>
    [ServiceContract]
    public interface IPromotionsService
    {

        [OperationContract]
        Collection<Promotion> GetPromotions();

    }
}
