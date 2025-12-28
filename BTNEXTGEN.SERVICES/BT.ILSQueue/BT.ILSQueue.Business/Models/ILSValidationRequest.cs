using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class ILSValidationRequest
    {
        public string TSOrgId { get; set; }
        public string ILSUrl { get; set; }
        public string ILSApiKey { get; set; }
        public string ILSApiSecret { get; set; }
        public string ILSLogin { get; set; }
        public int ILSValidationStatusId { get; set; }
        public string ILSValidationStatus { get; set; }
        public DateTime? ILSValidationDateTime { get; set; }
        public string ILSValidationDateTimeInString
        {
            get
            {
                return ILSValidationDateTime.HasValue ? ILSValidationDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : "";
            }
        }
        public string ILSValidationErrorMessage { get; set; }

        public string ILSUserDomain { get; set; }

        public string ILSUserAccount { get; set; }
    }
}
