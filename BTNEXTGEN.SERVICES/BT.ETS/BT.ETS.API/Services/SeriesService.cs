using BT.ETS.API.Request;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BT.ETS.API.Services
{
    public class SeriesService
    {


        IMongoClient _client;
        IMongoDatabase _dbProducts;
        IMongoDatabase _dbStandingOrders;

        IMongoCollection<BsonDocument> _profiledSeries;
        IMongoCollection<BsonDocument> _profiles;
        IMongoCollection<BsonDocument> _products;

        public SeriesService()
        {
            var connection = AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.MongoDBConnectionString);

            _client = new MongoClient(connection);

            _dbProducts = _client.GetDatabase(MongoDbHelper.ProductsDatabaseName);
            _products = _dbProducts.GetCollection<BsonDocument>(MongoDbHelper.ProductsCollectionName);

            _dbStandingOrders = _client.GetDatabase(MongoDbHelper.StandingOrdersDatabaseName);
            _profiledSeries = _dbStandingOrders.GetCollection<BsonDocument>(MongoDbHelper.ProfiledSeriesCollectionName);

        }

        internal async Task<List<DupCheckSeriesInfo>> GetDuplicateSeriesIdByTitle(DupCheckDetailRequest duplicateSeriesTitleRequest)
        {
            
            try
            {
                ObjectId profileObjectId;

                List<DupCheckSeriesInfo> data = await GetDuplicateSeriesIdData(duplicateSeriesTitleRequest);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        private async Task<List<DupCheckSeriesInfo>> GetDuplicateSeriesIdData(DupCheckDetailRequest duplicateSeriesTitleRequest)
        {
            var seriesIdList = await GetSeriesIdByBTKey(duplicateSeriesTitleRequest);

             
            var profiledSeriesDocuments = await GetProfiledSeriesDocumentBySeriesId(seriesIdList, duplicateSeriesTitleRequest);
            var duplicateProfiledSeriesList = new List<DupCheckSeriesInfo>();
            foreach (var profiledSeries in profiledSeriesDocuments)
            {
                var duplicateProfiledSeries = new DuplicateProfiledSeries();
                if (profiledSeries.Contains("_id"))
                    duplicateProfiledSeries.ProfiledSeriesId = profiledSeries["_id"].AsObjectId.ToString();
                if (profiledSeries.Contains("SeriesID"))
                    dupCheckInfo.SeriesId = profiledSeries["SeriesID"].AsString;
                duplicateProfiledSeriesList.Add(dupCheckInfo);
            }

            foreach (var seriesDuplicateTitle in seriesDuplicateTitleList)
            {
                foreach (var duplicateProfiledSeries in duplicateProfiledSeriesList)
                {
                    if (seriesDuplicateTitle.SeriesIdList.Contains(duplicateProfiledSeries.SeriesId))
                    {
                        seriesDuplicateTitle.DuplicateProfiledSeriesIdList += duplicateProfiledSeries.ProfiledSeriesId + ";";
                    }
                }
            }
            duplicateSeriesTitleResponse.SeriesDuplicateTitleList = seriesDuplicateTitleList;
            return duplicateSeriesTitleResponse;
        }

        private SeriesDuplicateTitle getProductDetailsFromProductDocument(BsonDocument productDocument)
        {
            var seriesDuplicateTitle = new SeriesDuplicateTitle();
            var seriesIdList = new List<string>();
            seriesDuplicateTitle.SeriesIdList = seriesIdList;
            seriesDuplicateTitle.DuplicateProfiledSeriesIdList = string.Empty;
            if (productDocument.Contains("_id"))
                seriesDuplicateTitle.BTKey = productDocument["_id"].AsString;
            if (productDocument.Contains("SeriesInformation"))
            {
                var seriesInformationDocuments = productDocument["SeriesInformation"].AsBsonArray;
                foreach (var value in seriesInformationDocuments)
                {
                    var seriesProducts = BsonSerializer.Deserialize<SeriesProducts>(value.ToJson());
                    seriesIdList.Add(seriesProducts.SeriesID);
                }
                seriesDuplicateTitle.SeriesIdList = seriesIdList;
            }
            return seriesDuplicateTitle;
        }

        private async Task<List<string>> GetSeriesIdByBTKey(DupCheckDetailRequest duplicateSeriesTitleRequest)
        {
            var filter = Builders<BsonDocument>.Filter.Ne("_id", duplicateSeriesTitleRequest.BTKey);

            int retryWaitTime = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MongoDBRetryWaitTime);
            int retries = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MongoDBMaxConnectionRetries);
            var productList = new List<BsonDocument>();
            var projection = Builders<BsonDocument>.Projection.Include("SeriesInformation");
            while (retries > 0)
            {
                try
                {
                    productList = await _products.Find<BsonDocument>(filter).Project(projection).ToListAsync<BsonDocument>();

                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }

            var seriesIdList = new List<string>();
            foreach (var productDocument in productList) 
            {
                if (productDocument.Contains("SeriesInformation"))
                {
                    var seriesInformationDocuments = productDocument["SeriesInformation"].AsBsonArray;
                    foreach (var value in seriesInformationDocuments)
                    {
                        var seriesProducts = BsonSerializer.Deserialize<SeriesProducts>(value.ToJson());
                        seriesIdList.Add(seriesProducts.SeriesID);
                    }
                }
            }
            return seriesIdList;
        }

        private async Task<List<BsonDocument>> GetProfiledSeriesDocumentBySeriesId(List<string> seriesIdList, DupCheckDetailRequest duplicateSeriesTitleRequest)
        {
            int retryWaitTime = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MongoDBRetryWaitTime);
            int retries = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MongoDBMaxConnectionRetries);
            var profiledSeriesList = new List<BsonDocument>();
            var duplicateProfiledSeriesIdList = new List<string>();
            var filter = Builders<BsonDocument>.Filter.In("SeriesID", seriesIdList)
                & Builders<BsonDocument>.Filter.Eq("RedundantProfileInformation.OrganizationID", duplicateSeriesTitleRequest.OrganizationId);
            var projection = Builders<BsonDocument>.Projection.Include("SeriesID");
            while (retries > 0)
            {
                try
                {
                    profiledSeriesList = await _profiledSeries.Find<BsonDocument>(filter).Project(projection).ToListAsync<BsonDocument>();

                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }


            return profiledSeriesList;
        }
    }
}