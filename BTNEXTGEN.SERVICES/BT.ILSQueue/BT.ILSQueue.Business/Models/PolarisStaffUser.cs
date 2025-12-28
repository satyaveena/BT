using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class StaffUserRequest
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
     

    }

    public class StaffUserResponse
    {
        public int PAPIErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
        public int PolarisUserID { get; set; }
        public int BranchID { get; set; }
        public string AuthExpDate { get; set; }
    }
}
