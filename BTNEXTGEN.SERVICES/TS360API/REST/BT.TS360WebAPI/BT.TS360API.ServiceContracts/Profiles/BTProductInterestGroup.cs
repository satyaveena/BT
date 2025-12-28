namespace BT.TS360API.ServiceContracts.Profiles
{
    public class BTProductInterestGroup
    {
        public BTProductInterestGroup()
        {
        }

        public string Id { get; set; }

        public string PIGName { get; set; }

        public string Description { get; set; }

        public string[] ProductTypeList { get; set; }

        public string[] MarketTypeList { get; set; }

        public string DisplaySequence { get; set; }
    }
}
