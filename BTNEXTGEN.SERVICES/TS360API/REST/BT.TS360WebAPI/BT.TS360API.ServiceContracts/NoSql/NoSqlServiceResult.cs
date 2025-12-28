using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.NoSql
{
    public class NoSqlServiceResult<T>
    {
        public T Data { get; set; }

        public NoSqlServiceStatus Status { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public NoSqlServiceResult()
        {
            Status = NoSqlServiceStatus.Success;
        }
    }
}
