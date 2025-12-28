namespace BT.TS360API.ServiceContracts
{
    public class SearchCdmsAdditionalUsersRequest : BaseRequest
    {
        public string cdmsListId { get; set; }
        public string keyword { get; set; }
        public int batchNo { get; set; }

        public string UserId { get; set; }
    }
}
