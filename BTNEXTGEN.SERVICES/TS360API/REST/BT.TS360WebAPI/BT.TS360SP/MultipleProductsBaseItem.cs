using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360SP
{
    public class MultipleProductsBaseItem : CMListItem
    {
        private string _btKeyList;
        private int _numberOfProducts;
        public string BTKeyList
        {
            get
            {
                return _btKeyList;
            }
            set
            {
                _btKeyList = value;
                if (_btKeyList != null)
                {
                    var splitedItems = _btKeyList.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var btKeyList = new List<string>();
                    foreach (var splitedItem in splitedItems)
                    {
                        var trimmedBtKey = splitedItem.Trim();
                        if (!btKeyList.Contains(trimmedBtKey))
                        {
                            btKeyList.Add(trimmedBtKey);
                        }
                    }
                    _numberOfProducts = btKeyList.Count;
                    _btKeyList = String.Join("|", btKeyList.ToArray());
                }
            }
        }

        public int NumberOfProducts
        {
            get { return _numberOfProducts; }
            set { _numberOfProducts = value; }
        }

        public List<string> BTKeys
        {
            get
            {
                if (this.BTKeyList != null)
                    return this.BTKeyList.Split('|').ToList();
                return new List<string>();
            }
        }  
    }
}
