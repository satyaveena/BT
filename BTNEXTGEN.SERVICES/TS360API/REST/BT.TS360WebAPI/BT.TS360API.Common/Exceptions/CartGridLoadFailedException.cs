using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Exceptions
{
    public class CartGridLoadFailedException : CartGridException
    {
        public CartGridLoadFailedException()
        {

        }

        public CartGridLoadFailedException(string message)
            : base(message)
        {

        }
    }
}
