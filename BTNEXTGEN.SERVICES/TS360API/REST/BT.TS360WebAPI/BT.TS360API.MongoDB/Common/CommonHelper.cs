using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.MongoDB.Common
{
    public static class StringHelper
    {
        public static bool EqualsIgnoreCase(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsIgnoreCase(string a, Enum b)
        {
            return EqualsIgnoreCase(a, b.ToString());
        }
    }
}
