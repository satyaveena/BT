
namespace BT.TS360API.Common.Models
{
    public class Result
    {
        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }

        public string ErrorMessage { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data;
    }
    //public class ResultSet<T> : Result
    //{
    //    public T Data;
    //    public int Count;
    //}
}
