using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Models
{
    public class InsertedCartResult
    {
        public List<CartResult> Carts { get; set; }
        public List<ErrorItem> ErrorItems { get; set; }

        public InsertedCartResult()
        {
            Carts = new List<CartResult>();
            ErrorItems = new List<ErrorItem>();
        }
    }

    public class CartResult
    {
        public string CartId { get; set; }
        public string CartName { get; set; }
        public string CartUrl { get; set; }
    }

    public class ErrorItem
    {
        public string BTKey { get; set; }
        public string Message { get; set; }
    }
}
