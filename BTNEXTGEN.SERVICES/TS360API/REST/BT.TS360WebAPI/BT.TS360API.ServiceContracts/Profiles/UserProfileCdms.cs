using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts.Profiles
{
    public class UserProfileCdms
    {
        public UserProfileCdms()
        {
        }
        public string Id { get; set; }
        public string CdmsUserLoginName { get; set; }

        public string UserName { get; set; }

        public string OrganizationName { get; set; }
    }
}
