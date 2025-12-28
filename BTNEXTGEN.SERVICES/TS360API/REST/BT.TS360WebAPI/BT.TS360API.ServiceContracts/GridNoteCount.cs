namespace BT.TS360API.ServiceContracts
{
    public class GridNoteCount
    {
        public string BTKey { get; set; }
        public string LineItemId { get; set; }
        public int NoteCount { get; set; }
        public int QuantityCount { get; set; }
        public int GridLineCount { get; set; }
        public int LineItemTotalQuantity { get; set; }
    }
}
