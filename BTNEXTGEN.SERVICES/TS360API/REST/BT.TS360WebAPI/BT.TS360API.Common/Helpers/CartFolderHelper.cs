using BT.TS360API.Common.DataAccess;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Helpers
{
    public class CartFolderHelper
    {
        public static CartFolder CreateCartFolderFromDataRow(DataRow dr)
        {
            var cartFolder = new CartFolder()
            {
                CartFolderId = dr["UserFolderID"] as string,
                CartFolderName = dr["Literal"] as string,
                ParentFolderId = dr["ParentUserFolderID"] as string,
                UserId = dr["u_user_id"] as string,
                FolderType = DataAccessHelper.ConvertToFolderType(dr["UserFolderTypeID"]),
                Sequence = (float)DataAccessHelper.ConvertTodouble(dr["Sequence"]),
                TotalCarts = DataAccessHelper.ConvertToInt(dr["BasketCount"])
            };
            return cartFolder;
        }
    }
}
