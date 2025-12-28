using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Constants
{
    public class BusinessExceptionConstants
    {

        public const String SUCCEED = "0";
        public const String UNEXPECTED_EXCEPTION = "500";
        public const String ITEMS_VALIDATION_FAILED = "200";
        public const String PRODUCTS_VALIDATION_FAILED = "203";
        public const String INVALID_ITEM = "Invalid item";
        private static Dictionary<string, string> _messages;

        static BusinessExceptionConstants()
        {
            _messages = new Dictionary<string, string>();
            _messages.Add("100", "Invalid parameter – '{0}'");
            _messages.Add("101", "Invalid or organization not found");
            _messages.Add("102", "Inactive organization");
            _messages.Add("103", "User not found");
            _messages.Add("104", "Invalid Cart Name");
            _messages.Add("105", "Missing User ID");
            _messages.Add("106", "Missing ESPLibrary ID");
            _messages.Add("107", "Missing ESPCart ID");
            _messages.Add("108", "Missing cart name");
            _messages.Add("109", "Inactive user");
            _messages.Add("110", "Date value must be between 1/1/1753 and 12/31/9999");
            _messages.Add("200", "Items validation failed");
            _messages.Add("201", "ESP LibraryID does not match user");
            _messages.Add("202", "No DupCheck information found");
            _messages.Add("203", "Products validation failed");
            _messages.Add("204", "No ProductPricing information found");
            _messages.Add("210", "Product lists beyond threshold value");
            _messages.Add("211", "User Preference of Dupe Check is not in defined set");
            _messages.Add("500", "Unexpected exception");
            _messages.Add("510", "Timeout exception");
            _messages.Add("520", "Invalid input request");
            _messages.Add("212", "Missing or invalid Dup Check Type");
            _messages.Add("213", "Invalid Dupe Check Preference");
            _messages.Add("214", "Invalid Dup Check Download Cart Type");
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
