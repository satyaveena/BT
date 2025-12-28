using System.Collections.Generic;
using System;
namespace BT.ETS.Business.Models
{
    public class GridTemplateResult
    {
        public string OrganizationId { get; set; } //GUID
        public string ESPBranchField { get; set; }
        public string ESPFundField { get; set; }
        public List<GridTemplate> GridTemplates { get; set; }


    }
    public class GridTemplate
    {
        public string GridTemplateId { get; set; } //GUID
        public string GridTemplateName { get; set; }
        public string GridTemplateDesc { get; set; }

        public List<GridTemplateLine> GridTemplateLines { get; set; }


    }
    public class GridTemplateLine
    {
        public string GridTemplateLineId { get; set; }
        public int Quantity { get; set; }
        public string AgencyId { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyDesc { get; set; }
        public string ItemTypeId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public string CollectionId { get; set; }
        public string CollectionCode { get; set; }
        public string CollectionDesc { get; set; }
        public string CallNumberText { get; set; }
        public string UserCode1Id { get; set; }
        public string UserCode1Code { get; set; }
        public string UserCode1Desc { get; set; }
        public string UserCode2Id { get; set; }
        public string UserCode2Code { get; set; }
        public string UserCode2Desc { get; set; }
        public string UserCode3Id { get; set; }
        public string UserCode3Code { get; set; }
        public string UserCode3Desc { get; set; }
        public string UserCode4Id { get; set; }
        public string UserCode4Code { get; set; }
        public string UserCode4Desc { get; set; }
        public string UserCode5Id { get; set; }
        public string UserCode5Code { get; set; }
        public string UserCode5Desc { get; set; }
        public string UserCode6Id { get; set; }
        public string UserCode6Code { get; set; }
        public string UserCode6Desc { get; set; }
    }


   
}
