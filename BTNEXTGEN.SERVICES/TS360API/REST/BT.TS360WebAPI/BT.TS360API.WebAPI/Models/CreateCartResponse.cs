using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{

    public class CreateCartResponse


    {

        public string CartName { get; set; }
        public string PurchaseURL { get; set; }
        public Collection<ItemResponseList> Items { get; set; }
        public bool IsSuccessful { get; set;  }
        public string ErrorMessage { get; set;  }
        public string ErrorType { get; set ; } 


    }
    public class ItemResponseList
    {
        public string ItemID { get; set;  }
        public bool IsSuccessful { get; set;  }
        public string Errormessage { get; set ; } 
        public string ErrorType { get; set ; } 

     }

}