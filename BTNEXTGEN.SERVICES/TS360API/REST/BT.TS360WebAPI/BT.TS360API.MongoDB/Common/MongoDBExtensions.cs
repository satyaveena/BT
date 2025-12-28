using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace BT.TS360API.MongoDB.Common
{
    public static class MongoDBExtensions
    {
        /// <summary>
        ///  Casts the BsonValue to a String. Returns Empty string if the BsonValue is Null.
        /// </summary>
        /// <param name="bsonValue"></param>
        /// <returns></returns>
        public static string AsStringEx(this BsonValue bsonValue)
        {
            var value = string.Empty;

            if (bsonValue != null && bsonValue != BsonNull.Value)
                value = bsonValue.AsString;

            return value;
        }
    }
}
