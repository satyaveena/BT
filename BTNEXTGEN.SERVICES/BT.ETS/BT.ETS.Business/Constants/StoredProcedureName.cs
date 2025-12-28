namespace BT.ETS.Business.Constants
{
    /// <summary>
    /// Class StoredProcedureName
    /// </summary>
    public static class StoredProcedureName
    {
        #region Public Member
        public const string GetEspOrgs = "procETSGetESPOrgs";
        public const string GetLoginIdsByOrgId = "procETSGetLoginIDsByOrgID";
        public const string InsertEtsCart = "procETSInsertCarts";
        public const string GetDupChecks = "procETSDupCheck";
        public const string GetDupCheckDetails = "procETSDupCheckDetails";
        public const string ValidateOrganizationUser = "procETSOrganizationUserValidations";
        public const string GetGridTemplate = "procETSGetOrganizationGridTemplates ";
        public const string GetPricingRequests = "procETSGetPricingRequests";

        public const string GetUserShipToAccounts = "procTS360UserGetShipToAccounts";
        
        #endregion

    }
}