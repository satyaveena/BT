namespace BT.TS360API.ServiceContracts
{
    public class ShareGroupMember
    {
        public string ShareGroupMemberId { get; set; }  // = BasketUserGroupMemberID in DB
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsOwner { get; set; }
        public bool HasContributionStage { get; set; }
        public bool HasRequisitionStage { get; set; }
        public bool HasReviewStage { get; set; }
        public bool HasAcquisitionStage { get; set; }
    }
}
