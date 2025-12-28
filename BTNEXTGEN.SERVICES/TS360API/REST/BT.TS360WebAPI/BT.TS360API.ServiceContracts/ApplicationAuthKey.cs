using BT.TS360Constants;
namespace BT.TS360API.ServiceContracts
{
    public class ApplicationAuthKey
    {
        public string PremiumServiceCode { get; set; }

        public string AuthenticationCode { get; set; }
    }

    public class SSORefererBranding
    {
        public string Id { get; set; }
        /// <summary>
        /// Ex: "Sustainable Shelves Program" or "BTCAT".
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Referer DomainURL.
        /// </summary>
        public string DomainURL { get; set; }

        /// <summary>
        /// Css Inline Styles for specific application.
        /// </summary>
        public string CssInlineStyles { get; set; }

        public string RefererHeaderImageUrl { get; set; }

        /// <summary>
        /// Login Header Text. Ex: "BTCAT Login".
        /// </summary>
        public string RefererLoginHeaderText { get; set; }

        public string ForgotPasswordHeaderText { get; set; }
        public string ForgotPasswordMainBodyText { get; set; }
        public string ForgotPasswordSubBodyText { get; set; }

        public string ForgotLoginIDHeaderText { get; set; }
        public string ForgotLoginIDMainBodyText { get; set; }
        public string ForgotLoginIDSubBodyText { get; set; }

        /// <summary>
        /// Footer HTML for right part only.
        /// Ex: <a href='http://www.baker-taylor.com/privacy.cfm' target='_blank'>Privacy Policy</a>
        /// </summary>
        public string RefererFooterHTML { get; set; }
    }
}
