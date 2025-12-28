using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Helpers
{
    public class ExtendedWebClient : WebClient
    {

        private int timeout;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }
        public ExtendedWebClient(Uri address, int timeout)
        {
            this.timeout = (int)TimeSpan.FromSeconds(timeout).TotalMilliseconds;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var objWebRequest = base.GetWebRequest(address);
            objWebRequest.Timeout = this.timeout;
            return objWebRequest;
        }

        public new async Task<string> UploadStringTaskAsyncEx(string address, string method, string data)
        {
            //base.UploadStringTaskAsync() never throw timeout excetion.
            return await RunWithTimeout(base.UploadStringTaskAsync(address, method, data));
        }

        private async Task<T> RunWithTimeout<T>(Task<T> task)
        {
            if (task == await Task.WhenAny(task, Task.Delay(timeout)))
                return await task;
            else
            {
                this.CancelAsync();
                throw new TimeoutException();
            }
        }
    }
}
