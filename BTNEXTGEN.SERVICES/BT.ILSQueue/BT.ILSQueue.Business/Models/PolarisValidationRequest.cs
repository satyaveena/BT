using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class PolarisValidationRequest : PolarisPORequest
    {
        public int Copies { get; set; }
        public List<ILSOrderLineItem> LineItems { get; set; }
    }
}
