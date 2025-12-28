using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.ServiceContracts;

namespace BT.TS360API.Common.Business
{
    public class ProductCatalogDAOManager
    {
        private ProductCatalogDAOManager()
        { }

        private static volatile ProductCatalogDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static ProductCatalogDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProductCatalogDAOManager();
                }

                return _instance;
            }
        }

        public async Task<Collection<AdditionalVersion>> GetAdditionalVersions(string btKey, string eMarketType,
            string eTier)
        {
            return await ProductCatalogDAO.Instance.GetAdditionalVersions(btKey, eMarketType, eTier);
        }
    }
}
