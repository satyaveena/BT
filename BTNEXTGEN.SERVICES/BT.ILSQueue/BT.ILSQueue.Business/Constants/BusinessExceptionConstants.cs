using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Constants
{
    public class BusinessExceptionConstants
    {

        public const String SUCCEED = "0";
        public const String UNEXPECTED_EXCEPTION = "500";
       
        private static Dictionary<string, string> _messages;

        static BusinessExceptionConstants()
        {
            _messages = new Dictionary<string, string>();
          
        }

        public static string Message(int code)
        {
            return Message(code.ToString());
        }
        public static string Message(string code)
        {
            return _messages.ContainsKey(code) ? _messages[code] : string.Format("Error Code not found -'{0}'", code);
        }
    }
}
