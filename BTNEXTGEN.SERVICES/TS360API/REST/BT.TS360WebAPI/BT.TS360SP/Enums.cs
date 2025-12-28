namespace BT.TS360SP
{
    public enum MarketType : int
    {

        None = 0,

        Invalid = 1,

        Retail = 2,

        PublicLibrary = 4,

        SchoolLibrary = 8,

        AcademicLibrary = 16,

        All = 32,
    }
    public enum ItemStatus : int
    {

        None = 0,

        Invalid = 1,
        Draft = 2,

        Approved = 4,

        Published = 8,

        Expired = 16
    }
    public enum Mode : int
    {

        None = 0,

        Invalid = 1,
        TurnOn = 2,

        TurnOff = 4,
    }
    public enum AudienceType : int
    {

        None = 0,

        Invalid = 1,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Children\'s - Grade 2-3, Age 7-8")]
        ChildrenSGrade23Age78 = 256,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Teen - Grade 10-12, Age 15-18")]
        TeenGrade1012Age1518 = 16,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Children\'s - Grade 3-4, Age 8-9")]
        ChildrenSGrade34Age89 = 512,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Children\'s - Kindergarten, Age 5-6")]
        ChildrenSKindergartenAge56 = 32,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Children\'s - Grade 1-2, Age 6-7")]
        ChildrenSGrade12Age67 = 128,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Children\'s - Grade 4-6, Age 9-11")]
        ChildrenSGrade46Age911 = 1024,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Teen - Grade 7-9, Age 12-14")]
        TeenGrade79Age1214 = 4,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Scholarly/Graduate")]
        ScholarlyGraduate = 8192,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Professional")]
        Professional = 16384,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Children\'s - Babies, Age 0-2")]
        ChildrenSBabiesAge02 = 2,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Children\'s - Toddlers, Age 2-4")]
        ChildrenSToddlersAge24 = 8,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "General Adult")]
        GeneralAdult = 64,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Scholarly/Associate")]
        ScholarlyAssociate = 4096,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Vocational/Technical")]
        VocationalTechnical = 2048,

        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Scholarly/Undergraduate")]
        ScholarlyUndergraduate = 32768,

        All = 65536,
    }
    
}
