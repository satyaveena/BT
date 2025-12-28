using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Exceptions
{
    [Serializable]
    public class PostBackResultException : Exception
    {
        public PostBackResultException(string errorMessage)
            : base(errorMessage)
        {
        }

        public PostBackResultException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
