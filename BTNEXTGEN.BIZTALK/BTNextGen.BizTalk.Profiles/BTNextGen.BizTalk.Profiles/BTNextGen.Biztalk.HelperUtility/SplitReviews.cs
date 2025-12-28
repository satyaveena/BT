using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTNextGen.Biztalk.HelperUtility
{
    [Serializable]
    public class SplitReviews
    {
        string[] items;
        List<string> list;
        public int Split(string s, char c)
        {
            items = s.Split(c);
            list = new List<string>(items);
            int iReturn = GetCount(list);
            return iReturn;

        }

        private int GetCount(List<string> list)
        {
            int iCount = list.Count;
            return iCount;

        }

        public string GetItem(int iIndex)
        {
            string strValue = list.ElementAt(iIndex);
            return strValue;

        }

    }
}
