using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

namespace BTNextGen.Services.Common
{
    /// <summary>
    /// Custom XElement Name Equality Comparer
    /// </summary>
    public class XElementNameComparer : IEqualityComparer<XElement>
    {
        #region IEqualityComparer<XElement> Members

        public bool Equals(XElement x, XElement y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(XElement obj)
        {
            return obj.Name.GetHashCode();
        }

        #endregion
    }
}
