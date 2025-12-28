using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    
    public class DCGridCode
    {
        
        public string ID { set; get; }

        
        public string GridFieldId { set; get; }

        
        public string Code { set; get; }

        
        public string Literal { set; get; }

        
        public string EffectiveDate { set; get; }

        
        public string ExpirationDate { set; get; }

        
        public string Disable { set; get; }

        
        public string NumOfUsers { set; get; }

        
        public string LineStatus { get; set; }

        
        public string Sequence { get; set; }

        
        public string Users { get; set; }

        
        public string IsUsing { set; get; }

        
        public string Delete { set; get; }

        
        public string IsExpiredOrFutureDate { get; set; }

        
        public string IsAuthorized { get; set; }

        
        public bool IsGetBackUserIds { get; set; }
    }

    
    public class DCGridData
    {

        
        public string RadGridClientID { get; set; }

        
        public List<DCGridLine> DCGridLines { get; set; }

        
        public List<DCGridField> DCGridFields { get; set; }

        
        public string LineItemID { get; set; }

        
        public List<DCNote> DCNotes { get; set; }

        
        public List<DCGridTemplate> DCGridTemplates { get; set; }

        
        public List<string> DeletedItems { get; set; }

        
        public string RadNoteClientID { get; set; }

        
        public string BTKey { get; set; }

        
        public string CommandArgument { get; set; }

        
        public string CommandArgument2 { get; set; }

        
        public string GridTemplateId { get; set; }

    }

    
    public class DCGridField
    {
        
        public string ID { set; get; }

        
        public string IDToName { set; get; }

        
        public string Name { set; get; }

        
        public string IsFreeText { set; get; }

        
        public List<DCGridCode> DCGridCodes { set; get; }

        
        public string Sequence { set; get; }

        
        public string LineStatus { get; set; }

        
        public string SOPGridFieldId { set; get; }

        
        public string FreeTextCharacterLimit { set; get; }

        
        public string LockedIndicator { set; get; }

        
        public string Disable { set; get; }

        
        public string UserViewAllGridCodesIndicator { set; get; }

        
        public string ValidateIndicator { set; get; }

        
        public List<string> GridCodeDeletedItems { set; get; }

        
        public string IsUsing { get; set; }

        
        public bool IsShowCode { get; set; }

        
        public string DefaultGridCodeId { get; set; }

        
        public string DefaultGridCodeText { get; set; }

        
        public List<DCGridCode> DCFullGridCodes { get; set; }

        
        public string GridFieldType { get; set; }

        
        public int SlipReportSequence { get; set; }

        
        public bool LoadedGridCodes { get; set; }

        
        public string LastESPVerificationDateString { get; set; }
    }

    
    public class DCGridFieldCode
    {
        
        public string ID { set; get; }

        
        public string GridFieldId { get; set; }

        
        public string GridFieldIdToName { get; set; }

        
        public string GridCodeId { get; set; }

        
        public string GridCode { get; set; }

        
        public string GridCodeText { get; set; }

        
        public string IsFreeText { get; set; }

        
        public string HasError { get; set; }

        
        public string ErrorCode { get; set; }

        
        public string IsAuthorized { get; set; }

        
        public string IsDisabled { get; set; }

        
        public string IsExpired { get; set; }

        
        public string IsFutureDate { get; set; }

        
        public string IsUsed { get; set; }

        
        public string GridFieldType { get; set; }
    }

    
    public class DCGridLine
    {
        
        public string ID { set; get; }

        
        public List<DCGridFieldCode> DCGridFieldCodes { set; get; }

        
        public string Quantity { set; get; }

        
        public string LastModifiedByUserId { get; set; }

        
        public string IsAuthorized { get; set; }

        
        public string LineStatus { get; set; }

        
        public string HasError { get; set; }

        
        public string ErrorCode { get; set; }

        
        public string LastModifiedByUserName { get; set; }

        
        public string ValidLine { get; set; }

        
        public string LastModifiedTime { get; set; }

        
        public string Sequence { get; set; }

         
        public string IsTempDisabled { get; set; }

    }

    
    public class DCGridTitleProperty
    {
         
        public string ClientID { set; get; }

         
        public string OnDemandClientID { set; get; }

         
        public string BTKey { set; get; }

         
        public List<DCGridLine> DCGridLines { set; get; }

         
        public string LineItemID { set; get; }

         
        public List<string> DeletedItems { set; get; }

         
        public List<DCNote> DCNotes { set; get; }

         
        public string NoteClientID { set; get; }

         
        public string ApplySharedNoneGrid { set; get; }

         
        public string IsLoadCompleted { set; get; }

         
        public string QuantityClientID { set; get; }

         
        public string HasGridLines { set; get; }

         
        public string IsEBook { set; get; }

         
        public string IsReadOnlyTitle { set; get; }
    }

    
    public class DCUserGridTemplate
    {
         
        public string ID { set; get; }

         
        public string UserLoginName { set; get; }

         
        public string UserDisplayName { set; get; }

         
        public string PermissionAll { set; get; }

         
        public string MarkUpdate { set; get; }

         
        public string UserID { set; get; }

         
        public bool Checked { set; get; }
    }

    
    public class DCNote
    {
         
        public string NoteText { set; get; }

         
        public string UserId { set; get; }

         
        public string UserAlias { set; get; }

         
        public string ModifiedDate { set; get; }

         
        public string BTKeyInPrimary { set; get; }

         
        public string NoteQuantity { set; get; }

         
        public string HaveQuantity { set; get; }

    }

    
    public class DCGridUser
    {
         
        public string UserId { set; get; }

         
        public string UserAlias { set; get; }
    }

    
    public class DCGridTemplate
    {
         
        public string TemplateId { set; get; }

         
        public string TemplateName { set; get; }

         
        public string IsDefault { set; get; }

         
        public string RowExpansionRight { set; get; }

    }

    
    public class DCGridNoteCount
    {
         
        public string LineItemId { set; get; }

         
        public string BTKey { set; get; }

         
        public string GridLineCount { set; get; }

         
        public string NoteCount { set; get; }

         
        public string QuantityCount { set; get; }

         
        public string RadGridClientId { set; get; }

         
        public string RadNoteClientId { set; get; }

         
        public string QuantityClientId { set; get; }

         
        public string OnDemandClientId { set; get; }

    }

    
    public class GridOnDemandData
    {
         
        public string OnDemandClientId { set; get; }

         
        public string GridClientId { set; get; }

         
        public string NoteClientId { set; get; }

         
        public string BTGridNoteHeaderClientId { set; get; }

         
        public string ControlHTML { set; get; }

         
        public string IsFirst { set; get; }

         
        public DCGridData DCGridData { set; get; }
    }

    
    public class GridOnDemandParams
    {
         
        public string CartId { set; get; }

         
        public string OrgId { set; get; }

         
        public string Size { set; get; }

         
        public string UserId { set; get; }

         
        public string IsEBook { set; get; }

         
        public string IsReadOnlyTitle { set; get; }

    }

    /////////////////////////////////////////////////////////NEW GRID IMPLEMENTATION////////////////////////////////////////
    #region New Grid Implementation
    
    public class DCGridHeaderData
    {
         
        public List<DCGridField> DCGridFields { get; set; }

         
        public DCGridLine UserDefaultGridLine { get; set; }

        /// <summary>
        /// Grid Lines of a specific line item which is firsly opened by user.
        /// This field may or may not present depending on pages.
        /// </summary>
         
        public List<DCGridLine> DCGridLines { get; set; }

         
        public DCGridTemplateData DCGridTemplateData { get; set; }
    }

    
    public class DCGridTemplateData
    {
         
        public List<DCGridTemplate> DCGridTemplates { get; set; }

         
        public bool HasDefaultGridTemplate { get; set; }

         
        public string DefaultGridTemplateId { get; set; }

         
        public string NewTemplateName { get; set; }
    }

    
    public class DCBundleGridData
    {
         
        public List<DCGridField> DCGridFields { get; set; }

         
        public DCGridLine UserDefaultGridLine { get; set; }

         
        public DCGridTemplateData DCGridTemplateData { get; set; }

        // 
        //public List<DCGridTemplate> DCGridTemplates { get; set; }

        // 
        //public bool HasDefaultGridTemplate { get; set; }

        // 
        //public string DefaultGridTemplateId { get; set; }

         
        public List<DCLineItemGridData> DCLineItemGridData { get; set; }

         
        public string SessionCacheKey { get; set; }
    }

    
    public class DCLineItemGridData
    {
         
        public string LineItemID { get; set; }

         
        public List<DCGridLine> DCGridLines { get; set; }

         
        public bool IsLoaded { get; set; }

         
        public bool HasGridLines { get; set; }
    }

    
    public class DCGridInputData
    {
         
        public string BTKey { set; get; }

         
        public string LineItemID { set; get; }

         
        public List<string> DeletedItems { set; get; }

         
        public List<DCNote> DCNotes { set; get; }

         
        public List<DCGridLine> DCGridLines { set; get; }

         
        public string ApplySharedNoneGrid { set; get; }

         
        public string HasGridLines { set; get; }

         
        public string IsEBook { set; get; }

         
        public string IsReadOnlyTitle { set; get; }
    }

    
    public class DCGridTemplateToPrint
    {
         
        public string TemplateName { set; get; }

         
        public string TemplateDescriptions { set; get; }

         
        public string NumberOfUsers { set; get; }

         
        public string CreatedBy { set; get; }

         
        public string LastModified { set; get; }

         
        public List<DCGridLine> DCGridLines { set; get; }
    }

    #endregion
}
