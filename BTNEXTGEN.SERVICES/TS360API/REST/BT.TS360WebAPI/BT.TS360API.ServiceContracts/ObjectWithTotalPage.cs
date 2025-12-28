namespace BT.TS360API.ServiceContracts
{
    public class ObjectWithTotalPage<T>
    {
        public T Data { get; set; }
        public int TotalSize { get; set; }
        public ObjectWithTotalPage()
        {
            TotalSize = 1;
        }
    }
}
