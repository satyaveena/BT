using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
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
        public MARCProfile() { }
        public bool isDelete { get; set; }
        public bool isInserted { get; set; }
        public bool isUpdate { get; set; }
        public string copiedMARCProfileId { get; set; }
        public bool HasInventoryRules { get; set; }
        public MARCProfile(int sequence)
        {
            Sequence = sequence;
        }
    }

    public class MARCRecord
    {
        public string BTKey { get; set; }
        public string BasketLineItemID { get; set; }
        public string Tag { get; set; }
        public string Indicator { get; set; }
        public string Data { get; set; }
        public string SortSequence { get; set; }
        public string Tag914w { get; set; }
        public string Tag943j { get; set; }
        public string Tag000 { get; set; }
    }
}
