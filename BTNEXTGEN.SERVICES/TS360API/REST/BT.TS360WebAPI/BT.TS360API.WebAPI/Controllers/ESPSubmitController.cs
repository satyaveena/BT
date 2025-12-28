using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BT.TS360API.WebAPI.Services;
using BT.TS360API.WebAPI.Models;

namespace BT.TS360API.WebAPI.Controllers
{
    public class ESPSubmitController : ApiController
    {
        private  ESPSubmitRepository espSubmitRepository;

        public ESPSubmitController()
        {
            this.espSubmitRepository = new ESPSubmitRepository();
        }

        // GET: api/ESPSubmit
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ESPSubmit/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ESPSubmit
        public ESPServiceResult<ESPServiceResponse> Post([FromBody] ESPServiceRequest espServiceRequest)
        {
            var data = espSubmitRepository.Submit(espServiceRequest);
            return data;
        }

        // PUT: api/ESPSubmit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ESPSubmit/5
        public void Delete(int id)
        {
        }
    }
}
