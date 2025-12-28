using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class ValidateUserResponse
    {
        public bool isSuccessful { get; set; }
        public string errorMessage { get; set; }
        public string errorType { get; set; }

    }
}