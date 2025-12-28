using System.Collections.Generic;
using System.Linq;

namespace BT.TS360API.ServiceContracts
{
    
    public class NoteClientObject
    {
        
        public string BTKey { get; set; }
        
        public string LineItemId { get; set; }
        
        public string NotesCount { get; set; }
        
        public string Note { get; set; }
        
        public string MyQty { get; set; }

        public NoteClientObject()
        {
            BTKey = "";
            LineItemId = "";
            NotesCount = "0";
            Note = "";
            MyQty = "0";
        }
    }


    public class FacetNode
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public string NavigateUrl { get; set; }

        public bool IsShowCheckBox { get; set; }

        public int Level { get; set; }

        public FacetNode Parent { get; set; }

        public List<FacetNode> Nodes { get; set; }

        public FacetNode()
        {
            Nodes = new List<FacetNode>();
            Parent = null;
            Level = 0;
        }

        public FacetNode(FacetNode input, bool isIncludeNode = false)
        {
            IsShowCheckBox = input.IsShowCheckBox;
            Level = input.Level;
            NavigateUrl = input.NavigateUrl;
            Text = input.Text;
            Value = input.Value;
            Parent = input.Parent;
            Nodes = new List<FacetNode>();

            if (isIncludeNode && input.Nodes != null)
                Nodes.AddRange(input.Nodes);
        }

        public void Add(FacetNode node)
        {
            Nodes.Add(node);
            node.Parent = this;
            node.Level = this.Level + 1;
        }

        public FacetNode FindNode(string value)
        {
            return value == Value ? this : Nodes.Select(node => node.FindNode(value)).FirstOrDefault(result => result != null);
        }
    }
    public class SearchFacetNode
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string NavigateUrl { get; set; }
        public bool Checkable { get; set; }
        public bool Expandable { get; set; }
        public bool Filteralbe { get; set; }
        public int Level { get; set; }
    }
    
    //public class SearchFacetArg
    //{
        
    //    public string Name { get; set; }
        
    //    public string Value { get; set; }

    //}
}
