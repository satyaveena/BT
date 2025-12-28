using Elmah;
using System.Collections;

namespace BT.CDMS.Business.Logger.ELMAHLogger
{
    public class FakeELMAHMongoLogger : ErrorLog
    {

        public FakeELMAHMongoLogger()
        {
             
        }

        public override ErrorLogEntry GetError(string id)
        {
            return null;
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            return 0;
        }

        public override string Log(Error error)
        {
            return string.Empty;
        }
    }
}
