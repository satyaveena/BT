using System;
using System.Collections;
using BTNextGen.Biztalk.Catalog.CommonUtil.Properties;

namespace BTNextGen.Biztalk.Catalog.CommonUtil
{
    public interface CommonMapper
    {
        string GetCatalogName(string BTKey);
        string GetInventoryName(string BTKey);
        string GetMinLimitbyIteration(int Iteration);
        string GetMaxLimitbyIteration(int Iteration);
        string GetValues(string Key);
    }

    public class CatalogUtil : CommonMapper
    {
        public string GetValues(string Key)
        {
            return Properties.Catalog.Default[Key].ToString();
        }
        public string GetCatalogName(string BTKey)
        {
            return this.ReadSetting(BTKey, 1);
        }

        public string GetInventoryName(string BTKey)
        {
            return this.ReadSetting(BTKey, 2);
        }

        public string GetMinLimitbyIteration(int Iteration)
        {
            return Properties.Catalog.Default.MinValues[Iteration].ToString();
        }

        public string GetMaxLimitbyIteration(int Iteration)
        {
           return Properties.Catalog.Default.MaxValues[Iteration].ToString();
        }


        string ReadSetting(String BTKey, int ReturnType)
        {
            int TotalCatalog = Properties.Catalog.Default.InventoryCat.Count;
            string[] CSCatalog = new string[TotalCatalog];
            string[] MinValues = new string[TotalCatalog];
            string[] MaxValues = new string[TotalCatalog];
            Int64 vBTKey;
            String vResult = "";

            vBTKey = Int64.Parse(BTKey);


            switch (ReturnType)
            {
                case 1:
                    Properties.Catalog.Default.CatalogName.CopyTo(CSCatalog, 0);
                    break;
                case 2:
                    Properties.Catalog.Default.InventoryCat.CopyTo(CSCatalog, 0);
                    break;

            }


            Properties.Catalog.Default.MinValues.CopyTo(MinValues, 0);
            Properties.Catalog.Default.MaxValues.CopyTo(MaxValues, 0);

            for (int icount = 0; icount < TotalCatalog; icount++)
            {
                if ((vBTKey >= Int64.Parse(MinValues[icount])) && (vBTKey <= Int64.Parse(MaxValues[icount])))
                {
                    vResult = CSCatalog[icount];
                }
            }
            return vResult;
        }

    }
}
