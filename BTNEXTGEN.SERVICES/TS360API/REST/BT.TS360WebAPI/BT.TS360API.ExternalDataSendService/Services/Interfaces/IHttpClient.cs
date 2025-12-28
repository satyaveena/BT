using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BT.SustShelves.WebAPI.Services.Interfaces
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);

        Task<string> GetStringAsync(string requestUri);

        void AddAuthenticationHeader(AuthenticationHeaderValue headerValue);
    }
}
