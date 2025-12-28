namespace BT.TS360API.ServiceContracts
{
    public class CreditCard
    {
        public string CreditCardId { get; set; }
        public int ExpirationYear { get; set; }
        public int ExpirationMonth { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardIdentifier { get; set; }
        public string BTCreditCardToken { get; set; }
        public string Alias { get; set; }
        public string BillingAddressId { get; set; }
        public string CreditCardType { get; set; }
    }
}
