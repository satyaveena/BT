using System;

namespace BT.TS360API.ServiceContracts
{
    public class ESPCode
    {
        public string GridCodeID { get; set; }
        public string Code { get; set; }
        public string Literal { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool ESPMatchIndicator { get; set; }
        public bool ActiveIndicator { get; set; }
    }
}
