using System;
using log4net.Layout;

namespace BT.Auth.Business.Logger.Log4Sites
{
    public abstract class MongoAppenderField
    {
        public String Name { get; set; }
        public IRawLayout Layout { get; set; }
    }
}