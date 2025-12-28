using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    //public class CommonGridTemplateLine
    //{
    //    public string ID { get; set; }
    //    public string GridTemplateID { get; set; }
    //    public string AgencyCodeID { get; set; }
    //    public int Qty { get; set; }
    //    public string ItemTypeID { get; set; }
    //    public string CollectionID { get; set; }
    //    public string CallNumberText { get; set; }
    //    public string UserCode1ID { get; set; }
    //    public string UserCode2ID { get; set; }
    //    public string UserCode3ID { get; set; }
    //    public string UserCode4ID { get; set; }
    //    public string UserCode5ID { get; set; }
    //    public string UserCode6ID { get; set; }
    //    public int Sequence { get; set; }
    //    public bool EnabledIndicator { get; set; }
    //    public bool IsTempDisabled { get; set; }
    //    public string CreatedBy { get; set; }
    //    public DateTime CreatedDateTime { get; set; }
    //    public string UpdatedBy { get; set; }
    //    public DateTime UpdatedDateTime { get; set; }

    //    public List<GridTemplateFieldCode> GridFieldCodeList { get; set; }
    //}

    //public class GridTemplateFieldCode
    //{
    //    public string GridTemplateLineId { get; set; }

    //    public string GridFieldId { get; set; }

    //    public string GridCodeId { get; set; }

    //    public string GridCode { get; set; }

    //    public string GridText { get; set; }

    //    public bool IsFreeText { get; set; }

    //    public bool IsAuthorized { get; set; }

    //    public bool IsExpired { get; set; }

    //    public bool IsFutureDate { get; set; }

    //    public bool IsDisabled { get; set; }

    //    public bool IsExpiredOrFutureDate { get; set; }

    //    public GridFieldType GridFieldType { get; set; }

    //    public GridTemplateFieldCode()
    //    {
    //        GridCodeId = string.Empty;
    //        GridCode = string.Empty;
    //        GridText = string.Empty;
    //    }
    //}

    public enum GridFieldType
    {
        Unknown = -1,
        AgencyCode = 0,
        ItemType = 1,
        Collection = 2,
        UserCode1 = 3,
        UserCode2 = 4,
        UserCode3 = 5,
        UserCode4 = 6,
        UserCode5 = 7,
        UserCode6 = 8,
        CallNumber = 9
    }
}
