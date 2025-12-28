namespace BT.TS360API.ServiceContracts
{
    public class ActiveCartItem
    {
        public string CartId { get; set; }
        public string CartName { get; set; }
        public bool IsCurrentCart { get; set; }
    }
}
