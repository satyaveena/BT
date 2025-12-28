using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

namespace BTNextGen.Services.Common
{
    public static class XmlConversions
    {
        # region Public Methods
        /// <summary>
        /// Converts an XmlElement to an XElement.
        /// </summary>
        /// <param name="xmlelement">The XmlElement to convert.</param>
        /// <returns>The equivalent XElement.</returns>
        public static XElement ToXElement(this XmlElement xmlelement)
        {
            return XElement.Load(xmlelement.CreateNavigator().ReadSubtree());
        }

        /// <summary>
        /// Converts an XElement to an XmlElement.
        /// </summary>
        /// <param name="xelement">The XElement to convert.</param>
        /// <returns>The equivalent XmlElement.</returns>
        public static XmlElement ToXmlElement(this XElement xelement)
        {
            return new XmlDocument().ReadNode(xelement.CreateReader()) as XmlElement;
        }

        #endregion
    }
}
