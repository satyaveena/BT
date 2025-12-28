using System;
using System.Collections.Generic;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    [Serializable]
    public class ComboBoxData
    {
        public Dictionary<string, object> Context { get; set; }
        public bool EndOfItems { get; set; }
        public ComboBoxItemData[] Items { get; set; }
        public string Message { get; set; }
        public int NumberOfItems { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class ComboBoxItemData : ControlItemData
    {
    }

    [Serializable]
    public class ControlItemData
    {
        public ControlItemData()
        {
            // item enabled by default
            Enabled = true;
        }

        public IDictionary<string, object> Attributes { get; set; }
        public bool Enabled { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
