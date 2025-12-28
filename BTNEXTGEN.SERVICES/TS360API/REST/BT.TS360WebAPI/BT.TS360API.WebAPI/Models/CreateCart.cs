using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Runtime.Serialization; 
using System.Xml.XPath; 
using System.Xml;

namespace BT.TS360API.WebAPI.Models
{
      [DataContract]
    
    public class CreateCart
    {

        //public string vendorAPIKey { get; set; }  
        //public string ABC { get; set;  }
          [DataMember(Order = 1)]
     
        public string UserName { get; set; }
          [DataMember(Order = 2)]

        public string UserPassword { get; set; }  
          
          [DataMember(Order = 3)]
        public string UserEmail { get; set; }
          [DataMember(Order = 4)]

        public string UserEmailCC { get; set; }
          
         [DataMember(Order = 5)]
        public string CartGroup { get; set; }
          [DataMember(Order = 6)]

        public string CartName { get; set; }
          [DataMember(Order = 7)]

        public string CartNote { get; set; }
          [DataMember(Order = 8)]

        public string TargetSystem { get; set; }
          [DataMember(Order = 9)]

          public Collection<ItemRequestList> Items { get; set; }


          public override string ToString()
          {

              string itemsString = ""; 
              if (Items != null)
              {
                  foreach(ItemRequestList item in Items)
                  { 
                      itemsString += string.Format(" {0}", item.ToString() );
                  }
              }


              return string.Format("UserName :{0}" + 
                                   " UserPassword :{1}" + 
                                   " UserEmail :{2} " + 
                                   " UserEmailCC :{3} " +
                                   " CartGroup :{4} " +
                                   " CartName :{5} " +
                                   " CartNote :{6} " + 
                                   " TargetSystem :{7} " + 
                                   "Items : {8} ", 
                                   UserName, UserPassword, UserEmail, UserEmailCC, CartGroup, CartName, CartNote, TargetSystem, itemsString);
          }

      
      
      }


      public class ItemRequestList
      {
          [DataMember(Order = 1)]
          public string ItemID { get; set; }
          [DataMember(Order = 2)]
          public string PurchaseOrderLineText { get; set; }
          [DataMember(Order = 3)]
          public string Quantity { get; set; }

          




          public override string ToString()
          {
              return string.Format(
                                   " ItemID :{0} " +
                                   " PurchaseOrderLineText :{1} " +
                                   " Quantity :{2} ",
                                   ItemID, PurchaseOrderLineText, Quantity);


          }


      }    





    
}