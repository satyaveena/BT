namespace BT.TS360API.ServiceContracts.Search
{
    public class ISBNLookUpLink
    {
        public string ISBN13Link { get; set; }
        public string ISBN10Link { get; set; }
        public ISBNLookUpLink()
        {
            ISBN10Link = string.Empty;
            ISBN13Link = string.Empty;
        }
        public ISBNLookUpLink(string isbn13, string isbn10)
        {
            ISBN10Link = isbn10;
            ISBN13Link = isbn13;
        }
    }
}
