using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace BT.ETS.WinService
{
    [RunInstaller(true)]
    public partial class ETSServiceInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceInstaller _serviceInstaller;
        private readonly ServiceProcessInstaller _serviceProcessInstaller;

        public ETSServiceInstaller()
        {
            InitializeComponent();

            _serviceInstaller = new ServiceInstaller();
            _serviceProcessInstaller = new ServiceProcessInstaller();

            _serviceInstaller.StartType = ServiceStartMode.Automatic;
            _serviceInstaller.ServiceName = "BTNextGen ETS Service";
            _serviceInstaller.Description = "The purpose of the ETS windows service application" +
                                            " is to get ETS requests from queue (like CartReceived, DupCheck or Product Pricing) and process.";

            _serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            Installers.Add(_serviceInstaller);
            Installers.Add(_serviceProcessInstaller);
        }
    }
}
