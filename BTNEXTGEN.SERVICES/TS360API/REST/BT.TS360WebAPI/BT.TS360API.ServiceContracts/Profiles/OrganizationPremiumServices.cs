namespace BT.TS360API.ServiceContracts.Profiles
{
    public class OrganizationPremiumServices
    {
        public string orgid { get; set; }
        public bool espEnabled { get; set; }
        public bool espRanking { get; set; }
        public bool espDistribution { get; set; }
        public bool espFundMonitoring { get; set; }
        public string espLibraryId { get; set; }
        public string espLibraryName { get; set; }
        public bool vipEnabled { get; set; }
        public string ESPCollectionHQLibraryID { get; set; }

    }
}
