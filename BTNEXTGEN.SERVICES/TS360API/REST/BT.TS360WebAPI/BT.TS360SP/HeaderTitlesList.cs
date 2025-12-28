using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class HeaderTitlesList : CMListBase<HeaderTitlesItem>
    {
        public HeaderTitlesList()
        {
            HasAdName = true;
        }

        protected override string GetListName()
        {
            return "HeaderTitles";
        }
       
        //protected override string GetViewFields()
        //{
        //    return CMConstants.DefaultFieldNames + "<FieldRef Name='ComingSoonCarousel' /><FieldRef Name='WhatsHot' /><FieldRef Name='InTheNews' />" +
        //           "<FieldRef Name='PopularBTeLists' /><FieldRef Name='NewReleaseViewAllWTTag' />";
        //}
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["AdName"],
                       item => item["ComingSoonCarousel"],
                       item => item["WhatsHot"],
                       item => item["InTheNews"],
                       item => item["PopularBTeLists"],
                       item => item["NewReleaseViewAllWTTag"]
                       ));
        }
    }
}
