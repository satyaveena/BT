using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BT.TS360.Services.Cybersource.Common.FileLogging
{
    public class FileLogRepository
    {
        private string _folder;
        private string _prefix;
        private string _fileName;

        public enum Level { INFO, ERROR };

        public FileLogRepository(string folder, string prefix)
        {
            _folder = folder;
            _prefix = prefix;

            _fileName = string.Format("{0}_{1}.txt", _prefix, DateTime.Now.ToString("yyyy-MM-dd"));
            //_fileName = string.Format("{0}_{1}.txt", _prefix, DateTime.Now.ToString("yyyy-MM-dd-HHmm"));
            //_fileName = string.Format("{0}_{1}.txt", _prefix, DateTime.Now.ToString("yyyy-MM-dd-HH0000"));
        }

        public void Write(string message, Level level)
        {
            // logging message
            string logFile = string.Format("{0}\\{1}", _folder, _fileName);
            string logwrite = "false";
            long logcnt = 0;

            do
            {
                try
                {
                    logcnt = logcnt + 1;
                    DateTime now = DateTime.Now;
                    FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine(string.Format("{0}\t{1}\t{2}", now, level.ToString() + " " + logcnt.ToString(), message));
                    sw.WriteLine();

                    sw.Close();
                    fs.Close();
                    logwrite = "true";
                }
                catch
                (IOException IOex)
                {
                    Thread.Sleep(333);

                }

            }
            while (logcnt < 4 && logwrite == "false");

        }


        public void WriteErrors(string message, Level level)
        {
            // logging message
            string logFile = string.Format("{0}\\{1}", _folder, _fileName);
            string logwrite = "false";
            long logcnt = 0;

            do
            {
                try
                {
                    logcnt = logcnt + 1;
                    DateTime now = DateTime.Now;
                    FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine(string.Format("{0}", message));
                    sw.WriteLine();

                    sw.Close();
                    fs.Close();
                    logwrite = "true";
                }
                catch
                (IOException IOex)
                {
                    Thread.Sleep(333);

                }

            }
            while (logcnt < 4 && logwrite == "false");

        }


    }
}
