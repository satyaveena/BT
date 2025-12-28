using System.Collections.Generic;
using System;
namespace BT.CDMS.Business.Models
{
    /// <summary>
    /// Class GridTemplate
    /// </summary>
    public class GridTemplate
    {
        public string GridTemplateId { get; set; }
        public string GridTemplateName { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Class CheckGridTemplateAccessRequest
    /// </summary>
    public class CheckGridTemplateAccessRequest
    {
        public string GridTemplateId { get; set; }
        public List<string> UserIdsList { get; set; }
    }

    /// <summary>
    /// Class CheckGridTemplateAccessResponse
    /// </summary>
    public class CheckGridTemplateAccessResponse
    {
        public string UserId { get; set; }
        public bool IsAccess { get; set; }
    }
}
