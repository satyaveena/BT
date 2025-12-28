using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class CommonBaseGridUserControl
    {

        #region Enum

        /// <summary>
        /// GridCodeStatus
        /// </summary>
        public enum GridCodeStatus
        {
            Unauthorized = -1,
            Disabled = 0,
            Activated = 1,
            Expired = 2,
            FutureDate = 3
        }

        public enum PageModeType
        {
            EditGridNote,
            ViewCartLines,
            CartDetails,
            SearchResults,
            ItemDetails,
            CartItemDetails,
            OriginalEntry,
            BatchEntry,
            PrintTemplateLines,
            ViewTemplateLines
        }

        /// <summary>
        /// ValidationType
        /// </summary>
        public enum ValidationType
        {
            NoneValidation = 0,
            GridCodeDropDownList = 1,
            RequiredField = 2,
            TwoDateRangeField = 3
        }

        /// <summary>
        /// MessageType
        /// </summary>
        public enum MessageType
        {
            AddNew,
            Save,
            ConfirmDelete,
            GridActionCopyTo360Clipboard,
            GridActionDelete,
            GridActionFindReplace,
            GridActionSaveAsTemplate,
            GridActionSaveAsDefault,
            GridActionLabel,
            AddGridLinesLabel,
            FindReplaceLabel,
            AddGridLinesDefaultGrid,
            AddGridLinesPasteFrom360Clipboard,
            AddGridLinesFromTemplate,
            NoGridLineSelected,
            QuantityTitle,
            GridSystemErrorMessage,
            GridCodeCannotBeBlank,
            SelectItem,
            ClipboardIsBlank,
            SaveAsTemplateNewName,
            SaveAsTemplateSuccessful,
            GridLinesError,
            GridNoteHeader,
            Clear
        }

        /// <summary>
        /// ControlType
        /// </summary>
        public enum ControlType
        {
            GridFieldDropDownList = 0,
            GridFieldFreeText = 1,
            GridQuantity = 2,
            GridSequence = 3,
            GridText = 4,
            GridDate = 5,
            GridLink = 6,
            GridBoolean = 7,
            NoteOwner = 8,
            NoteText = 9,
            GridLabel = 10,
            NoteQuantity = 11
        }

        /// <summary>
        /// LineStatus
        /// </summary>
        public enum LineStatus
        {
            View = 0,
            Add = 1,
            Edit = 2
        }

        #endregion

        #region Class Structure For Store Data

        /// <summary>
        /// UIGridLine
        /// </summary>
        public class UIGridLine
        {
            public string ID { set; get; }
            public List<UIGridFieldCode> UIGridFieldCodes { set; get; }
            public int Quantity { set; get; }
            public string LastModifiedByUserId { get; set; }
            public bool IsAuthorized { get; set; }
            public LineStatus LineStatus { get; set; }

            public UIGridLine()
            {

            }

            public UIGridLine(string ID, List<UIGridFieldCode> UIGridFieldCodes, int Quantity, string LastModifiedByUserId, bool IsAuthorized)
            {
                this.ID = ID;
                this.UIGridFieldCodes = UIGridFieldCodes;
                this.Quantity = Quantity;
                this.LastModifiedByUserId = LastModifiedByUserId;
                this.IsAuthorized = IsAuthorized;
                this.LineStatus = LineStatus.View;
            }
        }

        /// <summary>
        /// UIGridFieldCode
        /// </summary>
        public class UIGridFieldCode
        {
            public string ID { set; get; }
            public string GridFieldId { get; set; }
            public string GridFieldIdToName { get; set; }
            public string GridCodeId { get; set; }
            public string GridCode { get; set; }
            public string GridCodeText { get; set; }
            public bool IsFreeText { get; set; }

            public UIGridFieldCode()
            {
            }

            public UIGridFieldCode(string ID, string GridFieldId, string GridCodeId, string GridCode, string GridCodeText)
            {
                this.ID = ID;
                this.GridFieldId = GridFieldId;
                this.GridFieldIdToName = CommonFunction.FieldIdToName(GridFieldId);
                this.GridCodeId = GridCodeId;
                this.GridCode = GridCode;
                this.GridCodeText = GridCodeText;
            }

            public UIGridFieldCode(string ID, string GridFieldId, string GridCodeId, string GridCode, bool IsFreeText, string GridCodeText)
            {
                this.ID = ID;
                this.GridFieldId = GridFieldId;
                this.GridFieldIdToName = CommonFunction.FieldIdToName(GridFieldId);
                this.GridCodeId = GridCodeId;
                this.GridCode = GridCode;
                this.IsFreeText = IsFreeText;
                this.GridCodeText = GridCodeText;
            }
        }

        /// <summary>
        /// UIGridField
        /// </summary>
        public class UIGridField
        {
            private string callNumberText = "CallNumber";

            public string ID { set; get; }
            public string IDToName { set; get; }
            public string Name { set; get; }
            public int Sequence { set; get; }

            public bool IsFreeText
            {
                get { return string.Compare(GridFieldType, callNumberText, StringComparison.OrdinalIgnoreCase) == 0; }
            }
            public List<UIGridCode> UIGridCodes { set; get; }
            public List<UIGridCode> UIFullGridCodes { set; get; }
            public LineStatus LineStatus { get; set; }
            public int SOPGridFieldId { set; get; }
            public int FreeTextCharacterLimit { set; get; }
            public bool LockedIndicator { set; get; }
            public bool ActiveIndicator { set; get; }
            public bool DeletedIndicator { set; get; }
            public bool UserViewAllGridCodesIndicator { set; get; }
            public bool ValidateIndicator { set; get; }
            public bool ShowCode { set; get; }
            public string DefaultGridCodeId { set; get; }
            public string DefaultGridCodeText { set; get; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedDateTime { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDateTime { get; set; }
            public int SlipReportSequence { get; set; }
            public string GridFieldType { get; set; }
            public string LastESPVerificationDateString { get; set; }
            public bool IsESPFundCodeField { get; set; }
            public bool IsESPBranchCodeField { get; set; }

            public UIGridField()
            {
            }

            public UIGridField(string ID, string Name, bool IsFreeText, List<UIGridCode> UIGridCodes)
            {
                this.ID = ID;
                this.IDToName = CommonFunction.FieldIdToName(ID);
                this.Name = Name;
                this.UIGridCodes = UIGridCodes;
            }

            public UIGridField(string ID, int Sequence, string Name, bool IsFreeText, List<UIGridCode> UIGridCodes)
            {
                this.ID = ID;
                this.IDToName = CommonFunction.FieldIdToName(ID);
                this.Sequence = Sequence;
                this.Name = Name;
                this.UIGridCodes = UIGridCodes;
            }

            public UIGridField(string ID, string Name, bool IsFreeText, List<UIGridCode> UIGridCodes, byte Sequence)
            {
                this.ID = ID;
                this.IDToName = CommonFunction.FieldIdToName(ID);
                this.Name = Name;
                this.UIGridCodes = UIGridCodes;
                this.Sequence = Sequence;
            }
        }

        public class UIUserGridField
        {
            public List<CommonBaseGridUserControl.UIGridCode> UIGridCodes;
            public string UserGridFieldID { set; get; }
            public string GridFieldID { set; get; }
            public string GridFieldName { get; set; }
            public string GridFieldType { get; set; }
            public string DefaultName { get; set; }
            public string DisplayType { set; get; }
            public string DefaultGridCodeID { set; get; }
            public string DefaultGridText { set; get; }
            public bool IsFreeText { get; set; }
        }

        public class UIUserGridCode : UIGridCode
        {
            public string UserGridCodeID { get; set; }
            public string UserID { get; set; }
            public string GridFieldID { get; set; }
        }

        /// <summary>
        /// Grid Code structure
        /// </summary>
        public class UIGridCode
        {
            public string ID { set; get; }
            public string Code { set; get; }
            public string Literal { set; get; }
            public DateTime? EffectiveDate { set; get; }
            public DateTime? ExpirationDate { set; get; }
            public bool Disable { set; get; }
            public bool Delete { set; get; }
            public bool IsAuthorized { set; get; }
            public int Sequence { set; get; }
            public LineStatus LineStatus { get; set; }
            public string UserIDs { get; set; }
            public bool IsExpired
            {
                get
                {
                    if (!ExpirationDate.HasValue) return false;
                    return DateTime.Compare(ExpirationDate.Value, DateTime.Now) < 0;
                }
            }
            public bool IsFutureDate
            {
                get
                {
                    if (!EffectiveDate.HasValue) return false;
                    return DateTime.Compare(EffectiveDate.Value, DateTime.Now) >= 0;
                }
            }
            public bool IsExpiredOrFutureDate
            {
                get { return (IsExpired || IsFutureDate); }
            }
            public bool IsUsed { get; set; }
            public int UserCount { get; set; }
            public string GridFieldID { get; set; }

            public UIGridCode()
            {

            }

            public UIGridCode(string ID, string Code, string Literal, DateTime? EffectiveDate, DateTime? ExpirationDate, bool Disable, int UserCount, int Sequence)
            {
                this.ID = ID;
                this.Code = Code;
                this.Literal = Literal;
                this.EffectiveDate = EffectiveDate;
                this.ExpirationDate = ExpirationDate;
                this.Disable = Disable;
                this.UserCount = UserCount;
                this.Sequence = Sequence;

            }

            public UIGridCode(string ID, string Code, string Literal, bool Disable, int UserCount)
            {
                this.ID = ID;
                this.Code = Code;
                this.Literal = Literal;
                this.Disable = Disable;
                this.UserCount = UserCount;
            }

            public UIGridCode(string ID, string Code, string Literal, bool Disable)
            {
                this.ID = ID;
                this.Code = Code;
                this.Literal = Literal;
                this.Disable = Disable;
            }

        }

        /// <summary>
        /// Notes
        /// </summary>
        public class UINote
        {
            public string ID { set; get; }
            public string NoteText { set; get; }
            public string UserId { set; get; }
            public string UserAlias { set; get; }
            public DateTime ModifiedDate { set; get; }

            public UINote()
            {

            }

            public UINote(string ID, string NoteText, string UserId, DateTime ModifiedDate)
            {
                this.ID = ID;
                this.NoteText = NoteText;
                this.UserId = UserId;
                this.ModifiedDate = ModifiedDate;
            }
        }

        /// <summary>
        /// User
        /// </summary>
        public class UIGridUser
        {
            public string UserId { set; get; }
            public string UserAlias { set; get; }

            public UIGridUser()
            {
            }

            public UIGridUser(string UserId, string UserAlias)
            {
                this.UserId = UserId;
                this.UserAlias = UserAlias;
            }
        }

        /// <summary>
        /// GridValidation
        /// </summary>
        protected internal class GridValidation
        {
            public string ID { set; get; }
            public ValidationType Type { set; get; }
            public string GridControlName { set; get; }
            public string ErrorMessage { set; get; }
            public string OtherGridControlName { set; get; }

            public GridValidation(string ID, ValidationType Type, string GridControlName, string ErrorMessage)
            {
                this.ID = ID;
                this.Type = Type;
                this.GridControlName = GridControlName;
                this.ErrorMessage = ErrorMessage;
            }

        }

        /// <summary>
        /// GridColumnProperty
        /// </summary>
        public class GridColumnProperty
        {
            public string ControlName { set; get; }
            public string DataField { set; get; }
            public bool IsEditable { set; get; }
            public bool IsViewable { set; get; }
            public ControlType ControlType { set; get; }
            public UIGridField UIGridField { set; get; }
            public string ClientID { set; get; }
            public bool CanEditInViewMode { set; get; }
            public ValidationType ValidationType { set; get; }
            public bool AllowSort { set; get; }
        }

        /// <summary>
        /// GridTitleProperty
        /// </summary>
        public class GridTitleProperty
        {
            public string BTKey { set; get; }
            public string OnDemandClientID { set; get; }
            public string GridNoteHeaderClientID { get; set; }
            public string ClientID { set; get; }
            public string LineItemID { set; get; }
            public string QuantityClientID { set; get; }
            public string NoteClientID { set; get; }
            public string OriginalQuantity { set; get; }
            public string IsEBook { set; get; }
            public string IsLoadCompleted { set; get; }
            public string HasGridLines { set; get; }
            public string IsReadOnlyTitle { set; get; }
        }

        /// <summary>
        /// Grid Template Users
        /// </summary>
        public class UIUserGridTemplate
        {
            public string ID { set; get; }
            public string UserLoginName { set; get; }
            public string UserDisplayName { set; get; }
            public bool PermissionAll { set; get; }
        }

        public class UIErrorCode
        {
            public string ErrorCode { set; get; }
            public string ErrorMessage { set; get; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class SortProperty
        {
            public string ColumnName { set; get; }
            public ControlType ControlType { set; get; }
            public string SortType { set; get; }
            public string DisableColumnName { set; get; }
            public bool SwapDisable { set; get; }
        }

        #endregion

        private static class CommonFunction
        {
            private const string FieldIdPrefix = "FieldID";
            public static string FieldIdToName(string fieldId)
            {
                fieldId = fieldId.Replace("-", "");
                fieldId = fieldId.Replace("{", "");
                fieldId = fieldId.Replace("}", "");

                return fieldId.IndexOf(FieldIdPrefix, StringComparison.OrdinalIgnoreCase) == 0 ? fieldId :
                    string.Format("{0}_{1}", FieldIdPrefix, fieldId);
            }
        }

    }

    public class CommonGridTemplate
    {
        public string GridTemplateId { get; set; }
        public string Description { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfUsers { get; set; }
        public string OwnerUserID { get; set; }
        public string OwnerUserName { get; set; }
        public DateTime LastModified { get; set; }
        public string Name { get; set; }
        public bool EnabledIndicator { get; set; }
        public string CreatedBy { get; set; }
        public string DefaultBasketSummaryID { get; set; }
        public string RowExpansionRight { get; set; }
        public bool IsDefaultTemplate { get; set; }
        public bool IsGridDistribution { get; set; }
        public int? GridDistributionOption { get; set; }
    }

    public class CommonGridTemplateLine
    {
        public string ID { get; set; }
        public string GridTemplateID { get; set; }
        public string AgencyCodeID { get; set; }
        public int Qty { get; set; }
        public string ItemTypeID { get; set; }
        public string CollectionID { get; set; }
        public string CallNumberText { get; set; }
        public string UserCode1ID { get; set; }
        public string UserCode2ID { get; set; }
        public string UserCode3ID { get; set; }
        public string UserCode4ID { get; set; }
        public string UserCode5ID { get; set; }
        public string UserCode6ID { get; set; }
        public int Sequence { get; set; }
        public bool EnabledIndicator { get; set; }
        public bool IsTempDisabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public List<GridTemplateFieldCode> GridFieldCodeList { get; set; }
    }

    public class GridTemplateFieldCode
    {
        public string GridTemplateLineId { get; set; }

        public string GridFieldId { get; set; }

        public string GridCodeId { get; set; }

        public string GridCode { get; set; }

        public string GridText { get; set; }

        public bool IsFreeText { get; set; }

        public bool IsAuthorized { get; set; }

        public bool IsExpired { get; set; }

        public bool IsFutureDate { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsExpiredOrFutureDate { get; set; }

        public GridFieldType GridFieldType { get; set; }

        public GridTemplateFieldCode()
        {
            GridCodeId = string.Empty;
            GridCode = string.Empty;
            GridText = string.Empty;
        }

    }

    public class CommonCartGridLine
    {
        public string CartGridLineId { get; set; }

        public string LineItemId { get; set; }

        private bool? _hasError;
        /// <summary>
        /// Used for CartGrid validation
        /// </summary>
        public bool HasError
        {
            get
            {
                if (_hasError != null)
                {
                    return _hasError.Value;
                }

                _hasError = false;
                if (Quantity == 0)
                {
                    ErrorCode = "4"; // Grid lines with zero quantity.
                    _hasError = true;
                }

                if (GridFieldCodeList == null) return _hasError.Value;

                if (GridFieldCodeList.Any(cartGridLineFieldCode => cartGridLineFieldCode.GridFieldType != GridFieldType.CallNumber
                                                                   && cartGridLineFieldCode.GridCodeId == null))
                {
                    ErrorCode = "5"; // Grid lines with zero quantity.
                    _hasError = true;
                }

                return _hasError.Value;
            }
        }

        public string ErrorCode { get; set; }

        public string ErrorCodeFilter { get; set; }

        public string LastModifiedByUserId { get; set; }

        public string LastModifiedByUserName { get; set; }

        /// <summary>
        /// To Indicate that if the current login user is authorized again this GridLine.
        /// If there are one grid code, grid field the current user is not authorized, whole line will be unauthorized.
        /// </summary>
        public bool IsAuthorized { get; set; }

        public int Quantity { get; set; }
        public int Sequence { get; set; }

        public List<CartGridLineFieldCode> GridFieldCodeList { get; set; }
        //New Field in R3.1.1
        public string AgencyCodeID { get; set; }
        public string ItemTypeID { get; set; }
        public string CollectionID { get; set; }
        public string CallNumberText { get; set; }
        public string UserCode1ID { get; set; }
        public string UserCode2ID { get; set; }
        public string UserCode3ID { get; set; }
        public string UserCode4ID { get; set; }
        public string UserCode5ID { get; set; }
        public string UserCode6ID { get; set; }
    }

    public class CartGridLineFieldCode
    {
        public string GridFieldId { get; set; }

        public string GridCodeId { get; set; }

        public string GridCodeValue { get; set; }

        public bool IsFreeText { get; set; }

        public bool IsAuthorized { get; set; }

        public string GridTextValue { get; set; }

        public string CartGridLineId { get; set; }

        /// <summary>
        /// Position of the grid field in the de-normalized table.
        /// </summary>
        internal int GridFieldPosition { get; set; }

        /// <summary>
        /// Used for CartGrid validation
        /// </summary>
        public bool HasError { get; set; }

        public string ErrorCode { get; set; }

        /// <summary>
        /// New field in R3.1.1
        /// </summary>
        public bool IsExpired { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsExpiredOrFutureDate { get; set; }
        public bool IsFutureDate { get; set; }

        public GridFieldType GridFieldType { get; set; }
        public CartGridLineFieldCode()
        {
            GridCodeId = string.Empty;
            GridCodeValue = string.Empty;
            GridTextValue = string.Empty;
        }
    }
}
