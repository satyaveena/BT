using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Helpers
{
    public class CartGridDatabaseException : System.Exception
    {
        public const string UNAUTHORIZED_TO_EDIT_OR_REPLACE_GRID_LINES_MESSAGE = "You are not authorized to edit or replace some Grid Lines in the existing Grid Distribution.";
        public const string CART_DUPLICATE_NAME = "CART_DUPLICATE_NAME";

        public bool IsBusinessError { get; set; }

        public CartGridDatabaseException(string message)
            : base(message)
        {
        }
    }
}
