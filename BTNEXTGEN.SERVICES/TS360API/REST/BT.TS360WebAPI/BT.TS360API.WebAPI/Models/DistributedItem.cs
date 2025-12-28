using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class DistributedItem
    {

        public string ESPLibraryId { get; set; }

        public string CartId { get; set; }

        public DistJob Job { get; set; }

        public DistItem[] Items { get; set; }

        public override string ToString()
        {
            string itemsString = "";
            if (Items != null)
            {
                foreach (DistItem item in Items)
                {
                    itemsString += string.Format(" {0}", item.ToString() );
                }
            }

            return string.Format("ESPLibraryId:{0} | CartID:{1} | Jobs: {2} | Items: {3}", ESPLibraryId, CartId, (Job == null) ? "null" : Job.ToString(), itemsString);
        }
    }

    [DataContract]
    public class DistJob
    {
        public string JobId { get; set; }

        public string JobType { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime ProcessedAt { get; set; }

        public DateTime ReturnedAt { get; set; }

        public string Status { get; set; }

        [DataMember(Name = "api-version")]
        public string ApiVersion { get; set; }

        public override string ToString()
        {
            return string.Format("JobID:{0} | JobType:{1} | SubmittedAt:{2} | ProcessedAt:{3} | ReturnedAt: {4} | Status: {5} | api-version: {6}",
                JobId, JobType, SubmittedAt, ProcessedAt, ReturnedAt, Status, ApiVersion);
        }
    }
    public class DistItem
    {
        public string LineItemId { get; set; }

        public string VendorId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set;  }

        public string FundId { get; set; }

        public string FundCode { get; set; }



        public Grid[] Grid { get; set; }

        public DistRanking Ranking { get; set; }



        public override string ToString()
        {

            string gridItems = string.Empty;
            foreach (Grid grid in Grid)
            {
                gridItems += string.Format("{0} ", grid.ToString());
            }


            return string.Format("LineItemId:{0} | Quantity:{1} |FundId{2} |FundCode{3} |Grid:{4}", LineItemId, Quantity, FundId, FundCode,  (Grid == null) ? "null" : gridItems.ToString());
        }
    }

    public class Grid
    {
        public string BranchId { get; set; }

        public string BranchCode { get; set; }

        public int Quantity { get; set; }

        public override string ToString()
        {
            return string.Format("BranchId:{0} | BranchCode:{1} | BranchQuantity:{2}",
                                 (BranchId == null ? "null" : BranchId),
                                 (BranchCode == null ? "null" : BranchCode),
                                  Quantity);
        }
    }

    public class DistRanking
    {
        public double? Overall { get; set; }

        public double? Genre_Score { get; set; }

        public string Genre_Description { get; set; }

        public string Detail_Url { get; set; }

        public string OverallScoreType { get; set; }

        public DistRankingDetail[] Detail { get; set; }

        public int? DetailHeight { get; set; }
        public int? DetailWidth { get; set; }

        public override string ToString()
        {
            string detailItems = string.Empty;
            foreach (DistRankingDetail detail in Detail)
            {
                detailItems += string.Format("{0} ", detail.ToString());
            }

            return string.Format("Overall: {0} | Genre_Score: {1} | Genre_Description: {2} | OverallScoreType: {3} | Detail_Url: {4} | Detail: {5}", 
                Overall.ToString(), Genre_Score.ToString(), Genre_Description, OverallScoreType, Detail_Url, detailItems);
        }
    }

    public class DistRankingDetail
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