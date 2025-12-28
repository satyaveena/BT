using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class UserPreference 
    {
        public string Id { get; set; }//Object primary Key               

        private string _primaryCartId;
        public string PrimaryCartId
        {
            get { return _primaryCartId; }
            set
            {
                //SetChanged();
                _primaryCartId = value;
            }
        }

        private string _defaultGridTemplateId;
        public string DefaultGridTemplateId
        {
            get { return _defaultGridTemplateId; }
            set
            {
                //SetChanged();
                _defaultGridTemplateId = value;
            }
        }

        public int DefaultQuantity { get; set; }

        //protected override void PersistAsUpdate()
        //{
        //    CurrentDataAccessManager.UpdateUserPreference(this);
        //}
    }
}
