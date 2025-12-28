using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Exceptions
{
    public class CartGridException : System.Exception
    {
        public int ExceptionCode { get; private set; }

        public CartGridException()
        {

        }

        public CartGridException(string message)
            : base(message)
        {

        }

        public CartGridException(int exceptionCode, string message)
            : base(message)
        {
            ExceptionCode = exceptionCode;
        }
    }
}
