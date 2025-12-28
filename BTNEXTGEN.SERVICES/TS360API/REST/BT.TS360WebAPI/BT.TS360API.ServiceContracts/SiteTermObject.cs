namespace BT.TS360API.ServiceContracts
{

    public class SiteTermObject
    {

        public string Value
        { get; set; }


        public string Name
        { get; set; }

        public string BasketId { get; set; }

        public SiteTermObject()
        {
            this.Value = "";
            this.Name = "";
        }

        public SiteTermObject(string name, string value)
        {
            this.Value = value;
            this.Name = name;
        }

        public SiteTermObject(string name, string value, string basketId)
        {
            this.Value = value;
            this.Name = name;
            this.BasketId = basketId;
        }

    }
}
