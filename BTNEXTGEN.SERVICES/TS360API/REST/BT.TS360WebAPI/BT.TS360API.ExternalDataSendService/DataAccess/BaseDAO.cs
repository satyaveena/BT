using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ExternalDataSendService.Logging;

namespace BT.TS360API.ExternalDataSendService.DataAccess
{
    public abstract class BaseDAO
    {
        public abstract string ConnectionString
        {
            get;
        }

        protected virtual SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected virtual SqlCommand CreateSqlCommand(string commandName, SqlConnection sqlConnection)
        {
            return new SqlCommand(commandName, sqlConnection);
        }

        protected virtual SqlCommand CreateSqlSpCommand(string spName, SqlConnection sqlConnection)
        {
            var command = new SqlCommand(spName, sqlConnection) { CommandType = CommandType.StoredProcedure };

            var paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            var paramReturnValue = new SqlParameter("returnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            command.Parameters.Add(paramErrorMessage);
            command.Parameters.Add(paramReturnValue);

            return command;
        }

        protected virtual SqlCommand CreateSqlSpCommandNoErrorMessage(string spName, SqlConnection sqlConnection)
        {
            var command = new SqlCommand(spName, sqlConnection) { CommandType = CommandType.StoredProcedure };
            return command;
        }

        protected SqlParameter[] CreateSqlParamaters(int numberOfParams)
        {
            return new SqlParameter[numberOfParams];
        }

        /// <summary>
        /// Convert an object to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToString(object obj)
        {
            if (null != obj && DBNull.Value != obj)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        public static char ConvertToChar(object obj)
        {
            char returnValue = ' ';
            if (null != obj && DBNull.Value != obj)
            {
                char.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ConvertToInt(object obj)
        {
            int returnValue = 0;
            if (null != obj)
            {
                int.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean(object obj)
        {
            bool returnValue = false;
            if (null != obj && obj != DBNull.Value)
            {
                bool.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        public static T ConvertTo<T>(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                if (!row.IsNull(columnName))
                {
                    return row.Field<T>(columnName);
                }
            }
            return default(T);
        }

        public static decimal ConvertTodecimal(object obj)
        {
            decimal returnValue = 0;
            if (null != obj)
            {
                decimal.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        protected void HandleException(SqlCommand command)
        {
            var paramValue = command.Parameters["returnVal"].Value;
            var returnCode = -1;

            if (paramValue != null)
            {
                returnCode = DataAccessHelper.ConvertToInt(paramValue);
            }

            if (returnCode == 0)
                return;

            //Error
            paramValue = command.Parameters["@ErrorMessage"].Value;
            var errorMessage = paramValue != null ? paramValue.ToString() : "";

            var exception = new Exception(errorMessage);
            Logger.WriteLog(exception, "");

            throw exception;
        }

        //protected void HandleCartException(SqlCommand command)
        //{
        //    var paramValue = command.Parameters["returnVal"].Value;
        //    var returnCode = -1;

        //    if (paramValue != null)
        //    {
        //        returnCode = DataAccessHelper.ConvertToInt(paramValue);
        //    }

        //    if (returnCode == 0 || returnCode == 1000)
        //        return;

        //    //Error
        //    paramValue = command.Parameters["@ErrorMessage"].Value;
        //    var errorMessage = paramValue != null ? paramValue.ToString() : "";

        //    ////
        //    //if (command.CommandText.Equals(StoredProcedureName.ProcTs360QuickSearchAddProductToCart) && returnCode == 2)
        //    //    errorMessage = CartManagerException.CART_DUPLICATE_NAME;

        //    //var exception = new CartManagerException(errorMessage);

        //    //if (exception.Message.Equals(CartManagerException.CART_DUPLICATE_NAME)
        //    //    || exception.Message.Equals(CartManagerException.CART_FOLDER_NAME_DUPLICATED)
        //    //    || exception.Message.Equals(CartManagerException.CART_FOLDER_LIMITED_LEVEL)
        //    //    || exception.Message.Equals(CartManagerException.USER_HAS_NO_RIGHTS_TO_MOVE_OR_DELETE_ITEMS)
        //    //    || exception.Message.Equals("CART_FOLDER_NAME_DUPLICATED")
        //    //    || exception.Message.Equals("CART_FOLDER_LIMITED_TO_3_LEVELS")
        //    //    || exception.Message.Equals(CartManagerException.BASKET_STATE_IS_ORDERED_OR_SUBMITTED)
        //    //    || exception.Message.Contains("Lines to be Updated")
        //    //    || exception.Message.Contains("Cannot delete line items from a submitted")
        //    //    || exception.Message.Contains("Baskets not updated, one or more line items were in an ordered basket")
        //    //    || exception.Message.Contains(CartManagerException.INVALID_BASKET_NAME_USER_COMBINATION)
        //    //    || exception.Message.Contains("The userID is not currently a BasketUser")
        //    //    || exception.Message.Contains("Only copy function is allowed for this case")
        //    //    || exception.Message.Contains("The destination basket ID must be identified")
        //    //    || exception.Message.Contains("CANNOT_MERGE_TO_SUBMITTED_ORDERED_CART")
        //    //    || exception.Message.Contains("The basket is in a non open state")
        //    //    || exception.Message.Equals(CartManagerException.INVALID_USERID)
        //    //    || exception.Message.StartsWith("The UserID is not a BasketUser")
        //    //    || exception.Message.Equals(CartManagerException.NO_LINE_ITEMS_ROWS_UPDATED)
        //    //    || exception.Message.Equals(CartManagerException.CART_FOLDER_NOT_MATCH)
        //    //    || exception.Message.Equals(CartManagerException.BASKET_NOT_UPDATED_BECAUSE_OF_STATE)
        //    //    || exception.Message.Equals(CartManagerException.CART_ITEMS_EXCEED_THRESHOLD)
        //    //    || (exception.Message.StartsWith("The Target Cart") && exception.Message.EndsWith("must be in Contribution date to merge"))
        //    //    || (exception.Message.StartsWith("The Destination Cart") && exception.Message.EndsWith("must have a Status of OPEN before Copy Operation is permitted. Message 2001"))
        //    //    || (exception.Message.StartsWith("You must have Cart Ownership or ") && (exception.Message.EndsWith(". Message 2003") || exception.Message.EndsWith(". Message 2007") || exception.Message.EndsWith(" Rights to set the Cart to Active")))
        //    //    || exception.Message.Contains("You must be a Shared User of the Destination Cart")
        //    //    || exception.Message.Contains("You must be a Shared User of the Source Cart")
        //    //    || exception.Message.Equals("Adding this folder to the existing structure would result in a folder structure four levels deep.")
        //    //    || exception.Message.Equals("Archiving this folder would cause the archiving of the primary basket.")
        //    //    || exception.Message.Equals("Folders may not be added to a system folder.")
        //    //    || exception.Message.Equals("Delete of a system folder is not allowed.")
        //    //    || exception.Message.Equals("The user does not have an archived folder.")
        //    //    || exception.Message.EndsWith("Please remove an active cart before adding a new one.")
        //    //    || exception.Message.Equals("The Basket is in a Deleted State.")
        //    //    || exception.Message.StartsWith("You cannot set the Cart to Active during the")
        //    //    || exception.Message.EndsWith("a Non-Gridded Title into any Cart where that Title is already there as Gridded.")
        //    //    || exception.Message.Contains("The user(s) must be added to the Sharing Group for cart")
        //    //    || exception.Message.StartsWith("You cannot Add a Title to the Destination Cart")
        //    //    )
        //    //{
        //    //    exception.isBusinessError = true;
        //    //}
        //    //else if (exception.Message.Contains("BasketLineItemID does not exist")
        //    //    || exception.Message.Contains("Cannot insert the value NULL into column 'BasketOwnerName', table '@BS'; column does not allow nulls. INSERT fails."))
        //    //{
        //    //    //do nothing
        //    //}
        //    //else
        //        Logger.WriteLog(exception, ExceptionCategory.Order.ToString());

        //    throw exception;
        //}
    }
}
