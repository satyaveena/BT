using System;
using System.IO;
using BT.TS360.NoSQL.Data.Common.Constants;

namespace ILSWinService.Helper
{


    public class ThreadedLogger
    {

        private string _folder;
        private string _prefix;
        private string _fileName;

        private static object locker = new object();

        public ThreadedLogger(string folder, string prefix)
        {
            _folder = folder;
            _prefix = prefix;

            _fileName = string.Format("{0}_{1}.txt", _prefix, DateTime.Now.ToString("yyyy-MM-dd-HH0000"));
        }

        public void Write(string message, FileLoggingLevel level)
        {
            lock (locker)
            {
                string logFilePath = string.Format("{0}\\{1}", _folder, _fileName);
                using (FileStream file = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    DateTime now = DateTime.Now;
                    StreamWriter writer = new StreamWriter(file);
                    writer.Write(string.Format("{0}\t{1}\t{2}", now, level.ToString(), message));
                    writer.WriteLine();
                    writer.Flush();
                }
            }
        }


       
    }

}
