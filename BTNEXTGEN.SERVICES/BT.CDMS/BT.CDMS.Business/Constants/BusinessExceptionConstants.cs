using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.CDMS.Business.Constants
{
    public static class BusinessExceptionConstants
    {
        public const String INVALID_ORGNAME_CODE = "E0001";
        public const String INVALID_ORGNAME_MESSAGE = "Organization Name must be at least 2 characters";

        public const String INVALID_GRIDTEMPLATEID = "E0002";
        public const String INVALID_GRIDTEMPLATEID_MESSAGE = "Invalid argument for Grid Template Id";

        public const String INVALID_GRIDTEMPLATE_ACCESS_USERLISTS = "E0003";
        public const String INVALID_GRIDTEMPLATE_ACCESS_USERLISTS_MESSAGE = "No user Ids given to check grid template access";
    }
}
