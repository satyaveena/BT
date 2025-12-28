using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class ESPServiceRequest
    {
        public string CartId { get; set; }

        public string ESPType { get; set; }

        public string UserId { get; set; }

        public bool ShowJobURL { get; set; }
    }
}