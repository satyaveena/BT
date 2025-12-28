using BT.Auth.Business.Constants;
using log4net.Appender;
using log4net.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BT.Auth.Business.Logger.Log4Sites
{
    /// <summary>
    /// Class MongoDBAppender
    /// </summary>
    public class MongoDBAppender : AppenderSkeleton
    {
        #region Private Member
        private readonly List<MongoAppenderField> _fields = new List<MongoAppenderField>();
        private String ConnectionString { get; set; }
        private String ConnectionStringName { get; set; }
        private String CollectionName { get; set; }
        private String CertificateFriendlyName { get; set; }
        private Int64 ExpireAfterSeconds { get; set; }
        private String NewCollectionMaxDocs { get; set; }
        private String NewCollectionMaxSize { get; set; }
        private String ApplicationName{ get; set; }
        #endregion

        #region Public Member
        [Obsolete("Use ConnectionString")]
        public String Host { get; set; }
       [Obsolete("Use ConnectionString")]
        public Int32 Port { get; set; }
       [Obsolete("Use ConnectionString")]
        public String DatabaseName { get; set; }
       [Obsolete("Use ConnectionString")]
        public String UserName { get; set; }
       [Obsolete("Use ConnectionString")]
        public String Password { get; set; }

        public void AddField(MongoAppenderField field)
        {
            _fields.Add(field);
        }
        #endregion

        #region Protected Method
        protected override void Append(LoggingEvent loggingEvent)
        {
            IMongoCollection<BsonDocument> collection = GetCollection();
            collection.InsertOneAsync(BuildBsonDocument(loggingEvent));
            CreateExpiryAfterIndex(collection);
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            IMongoCollection<BsonDocument> collection = GetCollection();
            collection.InsertManyAsync(loggingEvents.Select(BuildBsonDocument));
            CreateExpiryAfterIndex(collection);
        }
        #endregion

        #region Private Method
        private IMongoCollection<BsonDocument> GetCollection()
        {
            IMongoDatabase db = GetDatabase();
            String collectionName = CollectionName ?? ApplicationConstants.LOG_TABLE_INFORMATION;

            EnsureCollectionExists(db, collectionName);

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>(collectionName);
            return collection;
        }

        private void EnsureCollectionExists(IMongoDatabase db, String collectionName)
        {
            if (!CollectionExists(db, collectionName))
            {
                CreateCollection(db, collectionName);
            }
        }

        private Boolean CollectionExists(IMongoDatabase db, String collectionName)
        {
            BsonDocument filter = new BsonDocument("name", collectionName);

            return db.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter })
                     .Result
                     .ToListAsync()
                     .Result
                     .Any();
        }

        private void CreateCollection(IMongoDatabase mongoDatabase, String collectionName)
        {
            CreateCollectionOptions createCollectionOptions = new CreateCollectionOptions();
            SetCappedCollectionOptions(createCollectionOptions);
            mongoDatabase.CreateCollectionAsync(collectionName, createCollectionOptions).GetAwaiter().GetResult();
        }

        private void SetCappedCollectionOptions(CreateCollectionOptions options)
        {
            UnitResolver unitResolver = new UnitResolver();

            Int64 newCollectionMaxSize = unitResolver.Resolve(NewCollectionMaxSize);
            Int64 newCollectionMaxDocs = unitResolver.Resolve(NewCollectionMaxDocs);

            if (newCollectionMaxSize <= 0) return;

            options.Capped = true;
            options.MaxSize = newCollectionMaxSize;

            if (newCollectionMaxDocs > 0)
            {
                options.MaxDocuments = newCollectionMaxDocs;
            }
        }

        private String GetConnectionString()
        {
            ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            ApplicationName = ApplicationConstants.APPLICATION_NAME;

            return connectionStringSetting != null ? connectionStringSetting.ConnectionString : ConnectionString;
        }

        private IMongoDatabase GetDatabase()
        {
            String connStr = GetConnectionString();

            if (String.IsNullOrWhiteSpace(connStr))
            {
                throw new InvalidOperationException("Must provide a valid connection string");
            }

            MongoUrl url = MongoUrl.Create(connStr);
            IMongoDatabase db = new MongoClient(connStr).GetDatabase(url.DatabaseName);

            return db;
        }

        //private SslSettings GetSslSettings()
        //{
        //    if (String.IsNullOrEmpty(CertificateFriendlyName)) return null;

        //    X509Certificate2 certificate = GetCertificate(CertificateFriendlyName);

        //    if (certificate.IsNull()) return null;

        //    SslSettings sslSettings = new SslSettings
        //    {
        //        ClientCertificates = new List<X509Certificate2> { certificate }
        //    };

        //    return sslSettings;
        //}

        //private X509Certificate2 GetCertificate(String certificateFriendlyName)
        //{
        //    X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
        //    store.Open(OpenFlags.ReadOnly);

        //    X509Certificate2Collection certificates = store.Certificates;
        //    X509Certificate2 certificateToReturn = certificates.Cast<X509Certificate2>().FirstOrDefault(certificate => certificate.FriendlyName.Equals(certificateFriendlyName));
        //    store.Close();
        //    return certificateToReturn;
        //}

        private BsonDocument BuildBsonDocument(LoggingEvent log)
        {
            if (_fields.Count == 0)
            {
                return BackwardCompatibility.BuildBsonDocument(log,ApplicationName);
            }

            BsonDocument doc = new BsonDocument();

            foreach (MongoAppenderField field in _fields)
            {
                Object value = field.Layout.Format(log);
                BsonValue bsonValue;

                // if the object is complex and can't be mapped to a simple object, convert to bson document
                if (!BsonTypeMapper.TryMapToBsonValue(value, out bsonValue))
                {
                    bsonValue = value.ToBsonDocument();
                }

                doc.Add(field.Name, bsonValue);
            }
            return doc;
        }

        private void CreateExpiryAfterIndex(IMongoCollection<BsonDocument> collection)
        {
            if (ExpireAfterSeconds <= 0) return;
            collection.Indexes.CreateOneAsync(Builders<BsonDocument>.IndexKeys.Ascending("timestamp"), new CreateIndexOptions()
            {
                Name = "expireAfterSecondsIndex",
                ExpireAfter = new TimeSpan(ExpireAfterSeconds * TimeSpan.TicksPerSecond)
            });
        }
        #endregion
    }
}