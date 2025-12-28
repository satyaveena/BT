using BT.Auth.Business.Constants;
using BT.Auth.Business.Logger.Log4Sites;
using Elmah;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BT.Auth.Business.Logger.ELMAHLogger
{
    /// <summary>
    /// ELMAHMongoLogger
    /// </summary>
    public class ELMAHMongoLogger : ErrorLog
    {
        #region Private Member
        private readonly String _connectionString;
        private readonly String _collectionName;
        private IMongoCollection<BsonDocument> _collection;
        private const Int32 DEFAULT_MAX_DOCUMENTS = Int32.MaxValue;
        private const Int32 DEFAULT_MAX_SIZE = 100 * 1024 * 1024;   // in bytes (100mb)

        private String NewCollectionMaxDocs { get; set; }
        private String NewCollectionMaxSize { get; set; }
        #endregion

        #region Constructor
        public ELMAHMongoLogger()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnStr"].ToString();
            ApplicationName = ApplicationConstants.APPLICATION_NAME;
            _collectionName = ApplicationConstants.LOG_TABLE_EXCEPTIONS;

            NewCollectionMaxDocs = DEFAULT_MAX_DOCUMENTS.ToString();
            NewCollectionMaxSize = DEFAULT_MAX_SIZE.ToString();

            Initialize();

        }
        #endregion

        #region Public Member
        public override String Name => "CDMS Auth ELMAH MongoDB Error Log";
        public virtual String ConnectionString => _connectionString;
        #endregion

        #region Method
        #region Public

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="error"></param>
        /// <returns>String</returns>
        public override String Log(Error error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            error.ApplicationName = ApplicationName;

            if (HttpContext.Current != null)
            {
                error.User = HttpContext.Current.User.Identity.Name;
            }

            error.Source = ApplicationConstants.ELMAH_ERROR_SOURCE;
            BsonDocument document = error.ToBsonDocument();

            ObjectId id = ObjectId.GenerateNewId();
            document.Add("_id", id);

            String errorXml = ErrorXml.EncodeString(error);
            document.Add("AllXml", errorXml);

            _collection.InsertOneAsync(document);

            return id.ToString();
        }

        /// <summary>
        /// GetError
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ErrorLogEntry</returns>
        public override ErrorLogEntry GetError(String id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (id.Length == 0) throw new ArgumentException(null, nameof(id));

            BsonDocument document = _collection.Find("{id:" + new ObjectId(id) + "}").FirstOrDefault();

            if (document == null)
                return null;

            Error error = BsonSerializer.Deserialize<Error>(document);

            return new ErrorLogEntry(this, id, error);
        }

        /// <summary>
        /// GetErrors
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="errorEntryList"></param>
        /// <returns>Int32</returns>
        public override Int32 GetErrors(Int32 pageIndex, Int32 pageSize, IList errorEntryList)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException(nameof(pageIndex), pageIndex, null);
            if (pageSize < 0) throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, null);

            List<BsonDocument> documents = _collection.Find(e => true).Sort("{ $natural: -1 }").Skip(pageIndex * pageSize).Limit(pageSize).ToList();

            foreach (BsonDocument document in documents)
            {
                String id = document["_id"].AsObjectId.ToString();
                Error error = BsonSerializer.Deserialize<Error>(document);
                error.Time = error.Time.ToLocalTime();
                errorEntryList.Add(new ErrorLogEntry(this, id, error));
            }

            return (Int32)_collection.Count(e => true);
        }

        /// <summary>
        /// GetCollectionLimit
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Int32</returns>
        public static Int32 GetCollectionLimit(IDictionary config)
        {
            Int32 result;
            return Int32.TryParse((String)config["maxDocuments"], out result) ? result : DEFAULT_MAX_DOCUMENTS;
        }

        /// <summary>
        /// GetCollectionSize
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Int32</returns>
        public static Int32 GetCollectionSize(IDictionary config)
        {
            Int32 result;
            return Int32.TryParse((String)config["maxSize"], out result) ? result : DEFAULT_MAX_SIZE;
        }
        #endregion
        #region Private

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            MongoUrl mongoUrl = MongoUrl.Create(_connectionString);
            IMongoDatabase database = new MongoClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName);
            EnsureCollectionExists(database, _collectionName);
            _collection = database.GetCollection<BsonDocument>(_collectionName);
        }

        /// <summary>
        /// EnsureCollectionExists
        /// </summary>
        /// <param name="db"></param>
        /// <param name="collectionName"></param>
        private void EnsureCollectionExists(IMongoDatabase db, String collectionName)
        {
            if (!CollectionExists(db, collectionName))
            {
                CreateCollection(db, collectionName);
            }
        }

        /// <summary>
        /// CollectionExists
        /// </summary>
        /// <param name="db"></param>
        /// <param name="collectionName"></param>
        /// <returns>Boolean</returns>
        private Boolean CollectionExists(IMongoDatabase db, String collectionName)
        {
            BsonDocument filter = new BsonDocument("name", collectionName);

            //return db.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter })
            //    .Result
            //    .ToListAsync()
            //    .Result
            //    .Any();
            return true;
        }

        /// <summary>
        /// CreateCollection
        /// </summary>
        /// <param name="mongoDatabase"></param>
        /// <param name="collectionName"></param>
        private void CreateCollection(IMongoDatabase mongoDatabase, String collectionName)
        {
            CreateCollectionOptions createCollectionOptions = new CreateCollectionOptions();
            SetCappedCollectionOptions(createCollectionOptions);
            mongoDatabase.CreateCollectionAsync(collectionName, createCollectionOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// SetCappedCollectionOptions
        /// </summary>
        /// <param name="options"></param>
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
        #endregion
        #endregion

    }
}
