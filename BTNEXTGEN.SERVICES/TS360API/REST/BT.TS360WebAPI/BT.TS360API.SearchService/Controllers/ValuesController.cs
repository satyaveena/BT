using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Http;
using BT.TS360API.Cache;
using BT.TS360API.ExternalServices;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts.Search;

namespace BT.TS360API.SearchService.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            var testEx = new Exception("for testing only");
            Logger.WriteLog(testEx, "TestCategory");

            ProfileService.Instance.GetOrganizationById("{0c0c7a24-8949-4ab4-8655-44647101821d}");

            //MarketingService.Instance.GetAllDiscounts("");

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(string id)
        {
            var temPS =
                CachingController.Instance.Read("__SearchResult{db1f3f20-f108-439b-80ba-2408af9cb842}") as
                    ProductSearchResults;

            //ProfileService.Instance.GetOrganizationById(id);
            return temPS == null ? "Empty" : temPS.TotalRowCount.ToString();
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
