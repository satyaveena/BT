using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class MARCProfile
    {
        public string MARCProfileId { get; set; }
        public string OrgId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDateTime { get; set; }
        public bool isDelete { get; set; }
        public bool isInserted { get; set; }
        public bool isUpdate { get; set; }
        public string copiedMARCProfileId { get; set; }
        public bool HasInventoryRules { get; set; }

        public MARCProfile() { }
        public MARCProfile(int sequence)
        {
            Sequence = sequence;
        }
    }
}
