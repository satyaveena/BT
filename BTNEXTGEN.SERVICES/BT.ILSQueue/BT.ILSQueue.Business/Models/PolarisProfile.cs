using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class PolarisProfile: ILSProfile
    {
        public string domain { get; set; }
        public string account { get; set; }
        public string password { get; set; }
    }
}
