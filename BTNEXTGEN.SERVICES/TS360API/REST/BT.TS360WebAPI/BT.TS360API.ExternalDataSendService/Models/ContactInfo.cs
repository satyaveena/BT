using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BT.TS360API.ExternalDataSendService.Models
{
    public class ContactInfo
    {
        public string ContactName { get; set; }
        [MaxLength(20, ErrorMessage = "Phone number is too long.")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
    }
}
