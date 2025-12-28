using System.Web;
using System.Web.Mvc;

namespace BT.TS360API.ExternalDataSendService
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
