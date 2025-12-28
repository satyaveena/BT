using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BT.ETS.Business.Models;
using BT.ETS.Business.Constants;

namespace BT.ETS.Business.Helpers
{
    public class SeriesHelper
    {

        #region Members
        private static SeriesHelper _instance;
        private const string LogCategory = "MONGODB";
        #endregion

        #region Constants

        private const string EtsDuplicateSeriesDetails = "standingorders/series/etsDuplicateSeriesDetails";
        private const string DuplicateSeries = "standingorders/series/duplicateSeriesTitles";

        #endregion Constants

        #region Properties
        private string _seriesUrl;
        private string _noSqlApiUrl;
        private int _timeout;
        private Uri SeriesUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_seriesUrl))
                {
                    _seriesUrl = AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.NoSqlApiUrlSeries);
                }
                return new Uri(_seriesUrl);
            }
        }

        private string NoSqlApiUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_noSqlApiUrl))
                    _noSqlApiUrl = AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.NoSqlApiUrl);
                return _noSqlApiUrl;
            }
        }

        private int Timeout
        {
            get
            {
                if (_timeout <= 0)
                    _timeout = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.WebRequestTimeOutInSec);
                return _timeout;
            }
        }

        #endregion

        #region Public Methods



        public static SeriesHelper GetInstance()
        {

            if (_instance == null) return new SeriesHelper();
            return _instance;
        }

        public DuplicateSeriesTitleResponse GetDuplicateSeriesTitle(List<string> btkeys, string organizationID)
        {
            var url = new Uri(this.NoSqlApiUrl + "/" + DuplicateSeries);
            var request = new DuplicateSeriesTitleRequest();
            request.OrganizationId = organizationID;

            int MaxDupCheckBatchSize_S = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MaxDupeCheckBatchSize_S);
            if (btkeys.Count <= MaxDupCheckBatchSize_S)
            {
                request.BTKeyList = btkeys;
                
                var response = GetDuplicateSeriesTitle(request, url);
                if (response != null)
                {
                    if (response.Status == NoSqlServiceStatus.Success)
                        return response.Data;
                    Logger.Write(LogCategory,
                            string.Format("MongoDb WebAPI Call For DuplicateSeriesTitle {0}, Error Code: {1}, Error Message {2}", response.Status,
                                response.ErrorCode, response.ErrorMessage));
                }
            }
            else
            {
                double loopCount = Math.Ceiling((double)btkeys.Count / MaxDupCheckBatchSize_S);
                var responseData = new DuplicateSeriesTitleResponse()
                {
                    SeriesDuplicateTitleList = new List<SeriesDuplicateTitle>()
                };
                
                for (int i = 0; i < loopCount; i++)
                {
                    request.BTKeyList = btkeys.Skip(i * MaxDupCheckBatchSize_S).Take(MaxDupCheckBatchSize_S).ToList();
                    
                    var tempResult = GetDuplicateSeriesTitle(request, url);

                    if (tempResult != null)
                    {
                        if (tempResult.Status == NoSqlServiceStatus.Success)
                        {
                            responseData.SeriesDuplicateTitleList.AddRange(tempResult.Data.SeriesDuplicateTitleList);
                        }
                        else
                        {
                            Logger.Write(LogCategory,
                            string.Format("MongoDb WebAPI Call For DuplicateSeriesTitle {0}, Error Code: {1}, Error Message {2}", tempResult.Status,
                                tempResult.ErrorCode, tempResult.ErrorMessage));
                        }
                    }
                          
                }
                if (responseData.SeriesDuplicateTitleList.Any())
                    return responseData;
                
            }

            return null;
        }

        private static NoSqlServiceResult<DuplicateSeriesTitleResponse> GetDuplicateSeriesTitle(DuplicateSeriesTitleRequest data, Uri webApiUri)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    foreach(var btkey in data.BTKeyList)
                    {
                        client.QueryString.Add("BTKeyList", btkey);
                    }
                    client.QueryString["BTKeyList"] = client.QueryString["BTKeyList"].Replace(",", "&BTKeyList=");
                    client.QueryString.Add("OrganizationId", data.OrganizationId);
                    var jss = new JavaScriptSerializer();
                    var response = client.DownloadString(webApiUri);
                    return jss.Deserialize<NoSqlServiceResult<DuplicateSeriesTitleResponse>>(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public async Task<List<ProfiledSeries>> GetDuplicateSeriesDetail(string btkey, string organizationId)
        {
            var url = new Uri(this.NoSqlApiUrl + "/" + SeriesHelper.EtsDuplicateSeriesDetails);

            DuplicateSeriesTitleRequest request = new DuplicateSeriesTitleRequest();
            request.OrganizationId = organizationId;
            request.BTKeyList = new List<string>();
            request.BTKeyList.Add(btkey);

            var serializer = new JavaScriptSerializer();
            string jsonRequest = serializer.Serialize(request);
            NoSqlServiceResult<List<ProfiledSeries>> res = null;
            using (var client = new ExtendedWebClient(url, this.Timeout))
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var response = await client.UploadStringTaskAsyncEx(url.AbsoluteUri, "POST", jsonRequest);
                res = serializer.Deserialize<NoSqlServiceResult<List<ProfiledSeries>>>(response);
            }

            return (res.Status == 0) ? res.Data : null;
        }

        #endregion
    }
}
