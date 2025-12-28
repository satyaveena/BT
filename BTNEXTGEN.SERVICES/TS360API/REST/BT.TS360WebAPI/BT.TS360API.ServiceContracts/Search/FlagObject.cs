
namespace BT.TS360API.ServiceContracts.Search
{
    public class FlagObject
    {
        public bool ShowToc { get; set; }

        public bool ShowMuze { get; set; }

        public FlagObject()
        {
            ShowToc = false;
            ShowMuze = false;
        }
        public FlagObject(bool toc, bool muze)
        {
            ShowToc = toc;
            ShowMuze = muze;
        }
    }
}
