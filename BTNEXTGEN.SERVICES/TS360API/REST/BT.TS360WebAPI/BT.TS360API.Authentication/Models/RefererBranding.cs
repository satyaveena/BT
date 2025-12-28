using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.Authentication.Models
{
    public class RefererBranding
    {
        public string Id { get; set; }
        public string ApplicationName { get; set; } // Ex: "Sustainable Shelves Program" or "BTCAT"
        public string DomainURL { get; set; }
        public string CssInlineStyles { get; set; }
        public string RefererHeaderImageUrl { get; set; }
        public string RefererFooterHTML { get; set; } // for footer right part only. Optional.

        // Login
        public string RefererLoginHeaderText { get; set; }   // Login Header Text

        // ForgotPassword
        public string ForgotPasswordHeaderText { get; set; }
        public string ForgotPasswordMainBodyText { get; set; }
        public string ForgotPasswordSubBodyText { get; set; }

        // ForgotLoginID
        public string ForgotLoginIDHeaderText { get; set; }
        public string ForgotLoginIDMainBodyText { get; set; }
        public string ForgotLoginIDSubBodyText { get; set; }
    }
}