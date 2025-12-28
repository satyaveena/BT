namespace BT.TS360API.ServiceContracts.Search
{
    public class OnHandInventory
    {
        public string COMQuantity { get; set; }
        public string MOMQuantity { get; set; }
        public string RENQuantity { get; set; }
        public string SOMQuantity { get; set; }

        public string PrimaryWareHouse { get; set; }
        public string SecondaryWareHouse { get; set; }
    }
}
