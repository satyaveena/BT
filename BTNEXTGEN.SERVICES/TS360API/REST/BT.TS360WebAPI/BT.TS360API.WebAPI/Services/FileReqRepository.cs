using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Services
{
    public class FileReqRepository
    {
        private string _folder;
        private string _prefix;
        private string _fileName;
        private string _guid; 

        public enum Level { INFO, ERROR };

        public FileReqRepository(string folder, string prefix, string guidd)
        {
            _folder = folder;
            _prefix = prefix;
            _guid = guidd;

            //_fileName = string.Format("{0}_{1}.txt", _prefix, DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
            _fileName = string.Format("{0}_{1}_{2}.txt", _prefix, DateTime.Now.ToString("yyyy-MM-dd"), _guid);
        }

        public void Write(string message, Level level)
        {
            // logging message
            DateTime now = DateTime.Now;

            string reqFile = string.Format("{0}\\{1}", _folder, _fileName);

            FileStream fs = new FileStream(reqFile, FileMode.Append, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(string.Format("{0}\t{1}\t{2}", now, level.ToString(), message));
            sw.WriteLine();

            sw.Close();
            fs.Close();
        }

    }
}