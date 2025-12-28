namespace BT.TS360API.ServiceContracts.Request
{
    public class DataFixPersitUserNoteRequest : BaseRequest
    {
        public string btKey { get; set; }
        public string userNote { get; set; }

        public string UserId { get; set; }
    }
}
