using log4net.Core;
using log4net.Util;
using MongoDB.Bson;
using System;
using System.Collections;

namespace BT.Auth.Business.Logger.Log4Sites
{
    /// <summary>
    /// Class BackwardCompatibility
    /// </summary>
    public class BackwardCompatibility
    {
        #region Method

        /// <summary>
        /// BuildBsonDocument
        /// </summary>
        /// <param name="loggingEvent"></param>
        /// <param name="applicationName"></param>
        /// <returns>BsonDocument</returns>
        public static BsonDocument BuildBsonDocument(LoggingEvent loggingEvent,String applicationName)
        {
            if (loggingEvent==null)
            {
                return null;
            }

            BsonDocument toReturn = new BsonDocument {
                {"ApplicationName", applicationName},
                {"TimeStamp", loggingEvent.TimeStamp.ToString("MM-dd-yyyy hh:mm:ss") },
                {"Level", loggingEvent.Level.ToString()},
                {"Host", Environment.MachineName},
                {"Source", loggingEvent.LoggerName},
                {"Message", loggingEvent.RenderedMessage},
                {"Sequence", loggingEvent.ThreadName},
                { "Domain", loggingEvent.Domain}
            };

            // exception information
            if (loggingEvent.ExceptionObject!=null)
            {
                toReturn.Add("Exception", BuildExceptionBsonDocument(loggingEvent.ExceptionObject));
            }

            // properties
            PropertiesDictionary compositeProperties = loggingEvent.GetProperties();

            if (compositeProperties==null || compositeProperties.Count <= 0)
                return toReturn;

            BsonDocument properties = new BsonDocument();

            foreach (DictionaryEntry entry in compositeProperties)
            {
                properties.Add(entry.Key.ToString(), entry.Value.ToString());
            }

            toReturn.Add("UserInformation", properties);

            return toReturn;
        }

        /// <summary>
        /// BuildExceptionBsonDocument
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>BsonDocument</returns>
        private static BsonDocument BuildExceptionBsonDocument(Exception ex)
        {
            BsonDocument toReturn = new BsonDocument {
                {"Message", ex.Message},
                {"Source", ex.Source},
                {"StackTrace", ex.StackTrace}
            };

            if (ex.InnerException!=null)
            {
                toReturn.Add("InnerException", BuildExceptionBsonDocument(ex.InnerException));
            }

            return toReturn;
        }
        #endregion
    }
}