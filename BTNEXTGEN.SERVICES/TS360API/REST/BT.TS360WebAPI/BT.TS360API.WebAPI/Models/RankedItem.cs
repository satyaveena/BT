using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class RankedItem
    {
        public string ESPLibraryId { get; set; }

        public string CartId { get; set; }

        public Job Job { get; set; }

        public Item[] Items {get; set;}

        public override string ToString()
        {
            string itemsString = "";
            if (Items != null)
            {
                foreach (Item item in Items)
                {
                    itemsString += string.Format(" {0}", item.ToString());
                }
            }

            return string.Format("ESPLibraryId:{0} | CartID:{1} | Jobs: {2} | Items: {3}", ESPLibraryId, CartId, (Job == null) ? "null" : Job.ToString(), itemsString);
        }
    }

    [DataContract]
    public class Job
    {
        public string JobId { get; set; }

        public string JobType { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime ProcessedAt { get; set; }

        public DateTime ReturnedAt { get; set; }

        public string Status { get; set; }

         [DataMember( Name = "api-version")]
        public string ApiVersion { get; set; }

        public override string ToString()
        {
            return string.Format("JobID:{0} | JobType:{1} | SubmittedAt:{2} | ProcessedAt:{3} | ReturnedAt: {4} | Status: {5} | api-version: {6}",
                JobId, JobType, SubmittedAt, ProcessedAt, ReturnedAt, Status, ApiVersion);
        }
    }
    public class Item
    {
        public string LineItemId { get; set; }

        public string VendorId { get; set; }

        public Ranking Ranking { get; set; }

        public override string ToString()
        {
            return string.Format("LineItemId:{0} | VendorId:{1} | Ranking:{2}", LineItemId, VendorId, (Ranking == null) ? "null" : Ranking.ToString());
        }
    }

    public class Ranking
    {
        public double? Overall { get; set; }

        public int Confidence { get; set; }

        public double? Genre_Score { get; set; }

        public string Genre_Description { get; set; }

        public string Detail_Url { get; set; }

        public string OverallScoreType { get; set; }

        public Detail[] Detail { get; set; }

        public int? DetailHeight { get; set; }
        public int? DetailWidth { get; set; }
        public override string ToString()
        {
            string detailItems = string.Empty;
            foreach (Detail detail in Detail)
            {
                detailItems += string.Format("{0} ", detail.ToString());
            }

            return string.Format("Overall: {0} | Genre_Score: {1} | Genre_Description: {2} | OverallScoreType: {3} | Detail_Url: {4} | Detail: {5}", 
                Overall.ToString(), Genre_Score.ToString(), Genre_Description, OverallScoreType, Detail_Url, detailItems);
        }
    }

    public class Detail
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Weight { get; set; }

        public int Confidence { get; set; }

        public double Value { get; set; }

        public override string ToString()
        {
            return string.Format("Name:{0} | Description: {1} | Weight: {2} | Confidence: {3} | Value: {4}", 
                                (Name == null ? "null" : Name), 
                                (Description == null ? "null" : Description), 
                                Weight, Confidence, Value);
        }
    }
}