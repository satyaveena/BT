using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common
{
    /// <summary>
    /// DAO Exception used to skip writing error log for business/expected error messages.
    /// </summary>
    public class DAOException : Exception
    {
        public bool IsBusinessError { get; set; }

        public DAOException(string message, bool isBusinessEx = false)
            : base(message)
        {
            IsBusinessError = isBusinessEx;
        }
    }
}
