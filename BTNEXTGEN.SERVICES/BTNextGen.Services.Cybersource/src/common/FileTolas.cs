using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BT.TS360.Services.Cybersource.Common.FileTolas
{
    public class FileTolas
    {
        private string _folder;
        private string _prefix;
        private string _fileName;

        public enum Level { INFO, ERROR };

        public FileTolas(string filename) 
        {
            
            _fileName = filename; 
        }

        public void Write(string message)
        {
            // logging message
            string tolasFile =  _fileName;
            string tolaswrite = "false";
            long logcnt = 0;

            do
            {
                try
                {
                    logcnt = logcnt + 1;
                    DateTime now = DateTime.Now;
                    FileStream fs = new FileStream(tolasFile, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine(message );

                    sw.Close();
                    fs.Close();
                    tolaswrite = "true";
                }
                catch
                (IOException IOex)
                {
                    Thread.Sleep(333);

                }

            }
            while (logcnt < 4 && tolaswrite == "false");

        }

        public void Create()
        {
            // logging message
            string tolasFile = _fileName;
            string tolaswrite = "false";
            long logcnt = 0;

            do
            {
                try
                {
                    logcnt = logcnt + 1;
                   FileStream fileStream = File.Create(_fileName );
                   fileStream.Close(); 
                    tolaswrite = "true";
                }
                catch
                (IOException IOex)
                {
                    Thread.Sleep(333);

                }

            }
            while (logcnt < 4 && tolaswrite == "false");

        }


    }
}
