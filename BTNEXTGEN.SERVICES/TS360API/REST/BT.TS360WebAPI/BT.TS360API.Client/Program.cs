using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BT.TS360API.Client
{
    public class RankedItem
    {
        public string ESPLibraryId { get; set; }

        public string CartId { get; set; }

        public Job Job { get; set; }

        public Item[] Items { get; set; }

        public override string ToString()
        {
            return string.Format("ESPLibraryId:{0} | CartID:{1} ; Jobs: {2}", ESPLibraryId, CartId, (Job == null) ? "null" : Job.ToString());
        }
    }

    public class Job
    {
        public string JobId { get; set; }

        public string JobType { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime ProcessedAt { get; set; }

        public DateTime ReturnedAt { get; set; }

        public string Status { get; set; }

        public override string ToString()
        {
            return string.Format("JobID:{0} | JobType:{1} | SubmittedAt:{2} | ProcessedAt:{3} | ReturnedAt: {4} | Status: {5}",
                JobId, JobType, SubmittedAt, ProcessedAt, ReturnedAt, Status);
        }
    }
    public class Item
    {
        public string LineItemId { get; set; }

        public Ranking Ranking { get; set; }
    }

    public class Ranking
    {
        public double Overall { get; set; }

        public int Confidence { get; set; }

        public Detail[] Detail { get; set; }
    }

    public class Detail
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Weight { get; set; }

        public int Confidence { get; set; }

        public double Value { get; set; }
    }

    public class PostDistributedCacheRequest
    {
        public string CacheName { get; set; }
        public string CacheKey { get; set; }
    }

    class Program
    {
        static void Main()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:8083/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //// HTTP GET
                //HttpResponseMessage response = await client.GetAsync("cart/ranked/1");
                //if (response.IsSuccessStatusCode)
                //{
                //    RankedItem rankedItem = await response.Content.ReadAsAsync<RankedItem>();
                //    Console.WriteLine("{0}\t{1}", rankedItem.ESPLibraryId, rankedItem.CartId);
                //}

                //// HTTP POST
                //Job job = new Job() { JobId="1234", JobType="rank", SubmittedAt=DateTime.Now, ProcessedAt=DateTime.Now, ReturnedAt=DateTime.Now, Status="returned"};
                //Detail detail1 = new Detail() { Name = "authorSeries", Description = "Author/Series", Weight = 50, Confidence = 75, Value = 84.459 };
                //Detail detail2 = new Detail() { Name = "authorBisac", Description = "Author/Bisac", Weight = 50, Confidence = 75, Value = 74.160 };
                //Detail[] detail = new Detail[2];
                //detail[0] = detail1;
                //detail[1] = detail2;
                //Ranking ranking = new Ranking() { Overall = 94.156, Confidence = 75, Detail = detail };
                //Item item1 = new Item() { LineItemId = "10766130eeeeeee", Ranking = ranking };
                //Item[] items = new Item[1];
                //items[0] = item1;
                //RankedItem newRank = new RankedItem() { ESPLibraryId = "clientlibrary1", CartId = "clientcart1", Job = job, Items = items };
                
                //response = await client.PostAsJsonAsync("cart/ranked", newRank);
                //if (response.IsSuccessStatusCode)
                //{
                    
                //}
            }
        }
    }
}
