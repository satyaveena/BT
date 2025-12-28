using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public interface ICMListItem
    {
        string GetAdName();

        void Initialize(ListItem item);
    }
}
