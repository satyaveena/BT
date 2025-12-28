namespace BT.TS360API.Common.Constants
{
    public class OrderResources
    {
        public static string CartFolder_CreateCartWithNotUniqueName = "The cart name must be unique.";
        public static string CART_ID_NULL = "Please select a cart.";
        public static string QuotedPrice = "(Quoted Price)";
        public static string ILS_ErrorOccured = "An error occurred, please try again.";
        public static string ILS_ErrorMoveCartFormat = "{0} items in your cart have errors, and we cannot process them. These items have been moved to a new cart {1}.";
        public static string ILS_AllItemsCartError = "All items in your carts have error and we cannot be process.";
        public static string ILS_ConnectFailureError = "We are unable to connect to server at this time. Please try again later.";
        public static string ILS_InvalidLoginError = "An invalid ILS Login ID was submitted. Please check your ILS Login ID and resubmit the cart.";
        public static string ILS_InvalidFundAndLocationCodeError = "Invalid Fund or Location Codes were found. Download and review the error file to resolve the issue and resubmit the cart.";
        public static string ILS_InvalidFundLocationAndCollectionCodeError = "Invalid Fund, Location or Collection Codes were found. Download and review the error file to resolve the issue and resubmit the cart.";
        public static string ILS_InvalidVendorCode = "Invalid ILS Vendor Code. Re-enter and submit the cart again.";
        public static string ILS_InvalidLocationCode = "Invalid ILS Location Code. Re-enter and submit the cart again.";

        public static string ILS_ValidationInvalidLoginError = "Invalid Login. ";
        public static string ILS_ValidationSucess = "Your API information has been validated and saved successfully.";
        public static string ILS_ValidationInvalidURLError = "Invalid URL. ";
        public static string ILS_ValidationInvalidAPISecretError = "Invalid API key or API Secret."; //"Invalid API Key" + "<br/>" + "Invalid API Secret";
        public static string ILS_ValidationUnexpectedError = "Unexpected error. Validation was not completed. Please try again.";
        public static string ILS_SaveUnexpectedError = "Unexpected error. Please try again.";
    }
}
