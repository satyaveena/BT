using System;

namespace BT.TS360API.Common
{
    public class CartManagerException : Exception
    {
        public const string INVALID_PARAMETERS = "INVALID_PARAMETERS";

        //Business
        public const string USER_ID_NULL = "USER_ID_NULL";
        public const string ORGANIZATION_ID_NULL = "ORGANIZATION_ID_NULL";
        public const string PO_NUMBER_INVALID = "PO_NUMBER_INVALID";
        public const string DELETED_CART_ID_NULL = "DELETED_CART_ID_NULL";
        public const string SELECTED_CART_ID_NULL = "SELECTED_CART_ID_NULL";
        public const string DESTINATION_FOLDER_ID_NULL = "DESTINATION_FOLDER_ID_NULL";
        public const string SOLD_TO_ID_NULL = "SOLD_TO_ID_NULL";

        public const string CART_DUPLICATE_NAME = "CART_DUPLICATE_NAME";
        public const string CART_CREATE_FAILED = "CART_CREATE_FAILED";
        public const string CART_ID_NULL = "CART_ID_NULL";
        public const string CART_NAME_NULL = "CART_NAME_NULL";
        public const string DESTINATION_CART_ID_NULL = "DESTINATION_CART_ID_NULL";

        public const string INVALID_CART_CHECK_TYPE_VALUE = "INVALID_CART_CHECK_TYPE_VALUE";
        public const string ITEM_KEY_NULL = "ITEM_KEY_NULL";

        public const string CART_FOLDER_NAME_DUPLICATED = "CART_FOLDERNAME_DUPLICATED";
        public const string CART_FOLDER_LIMITED_LEVEL = "CART_FOLDER_LIMITED_LEVEL";
        public const string CART_FOLDER_ARCHIVE_PRIMARY = "CART_FOLDER_ARCHIVE_PRIMARY";
        public const string CART_FOLDER_NOT_MATCH = "CART_FOLDER_NOT_MATCH";
        public const string CART_FOLDER_ID_NULL = "CART_FOLDER_ID_NULL";
        public const string CART_FOLDER_NAME_NULL = "CART_FOLDER_NAME_NULL";
        public const string BASKET_NOT_UPDATED_BECAUSE_OF_STATE = "Baskets not updated, one or more baskets were not in an open , downloaded or quoted state.";
        public const string NO_LINE_ITEMS_ROWS_UPDATED = "No BasketLineItems rows were updated.";

        //Data
        public const string CART_ID_DATA_NULL = "CART_ID_DATA_NULL";
        public const string CART_ACCOUNT_ID_DATA_NULL = "CART_ACCOUNT_ID_DATA_NULL";
        public const string CART_ACCOUNT_BASKET_ID_DATA_NULL = "CART_ACCOUNT_BASKET_ID_DATA_NULL";
        public const string PRIMARY_CART_DATA_RETRIEVE_FAILED = "PRIMARY_CART_DATA_RETRIEVE_FAILED";
        public const string CART_DATA_RETRIEVE_FAILED = "CART_DATA_RETRIEVE_FAILED";
        public const string INCORRECT_CART_DATA = "INCORRECT_CART_DATA";

        public const string CART_LINE_QUANTITY_INVALID = "CART_LINE_QUANTITY_INVALID";
        public const string CART_COPY_TITLE_EXISTING = "CART_COPY_TITLE_EXISTING";
        public const string CART_LINE_ITEM_NULL = "CART_LINE_ITEM_NULL";

        public const string CART_COMMON_SP_EXCEPTION = "CART_COMMON_SP_EXCEPTION";
        public const string CART_LINEITEM_LIMITATION = "CART_LINEITEM_LIMITATION";
        public const string BASKET_STATE_IS_ORDERED_OR_SUBMITTED = "BASKET_STATE_IS_ORDERED_OR_SUBMITTED";

        public const string USER_HAS_NO_RIGHTS_TO_MOVE_OR_DELETE_ITEMS = "USER_HAS_NO_RIGHTS_TO_MOVE_OR_DELETE_ITEMS";
        public const string CART_ITEMS_EXCEED_THRESHOLD = "CART_ITEMS_EXCEED_THRESHOLD";
        public const string BASKET_STATE_NOT_OPEN = "BASKET_STATE_NOT_OPEN";
        public const string BASKET_IS_SHARED = "BASKET_IS_SHARED";
        public const string CART_HAS_NO_ITEMS = "CART_HAS_NO_ITEMS";
        public const string INVALID_USER_ID = "INVALID_USER_ID";
        public const string INVALID_USERID = "Invalid UserID.";
        public const string INVALID_BASKET_NAME_USER_COMBINATION = @"Invalid Basket name\user combination";

        public bool isBusinessError { get; set; }
        public CartManagerException(string message, bool isBusinessEx = false)
            : base(message)
        {
            isBusinessError = isBusinessEx;
        }
    }
}
