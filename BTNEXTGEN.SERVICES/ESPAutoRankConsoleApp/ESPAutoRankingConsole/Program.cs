using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPAutoRankingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //log4net.Config.BasicConfigurator.Configure();
            log4net.Config.XmlConfigurator.Configure();

            var controller = new ESPAutoRankController();
            controller.DoEspAutoRankSubmit();
        }
    }
}
