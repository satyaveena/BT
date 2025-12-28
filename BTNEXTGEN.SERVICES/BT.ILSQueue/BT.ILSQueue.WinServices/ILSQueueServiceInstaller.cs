using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

using System.ServiceProcess;

namespace BT.ILSQueue.WinServices
{
    [RunInstaller(true)]
    public partial class ILSQueueServiceInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceInstaller _serviceInstaller;
        private readonly ServiceProcessInstaller _serviceProcessInstaller;
        public ILSQueueServiceInstaller()
        {
            InitializeComponent();

            _serviceInstaller = new ServiceInstaller();
            _serviceProcessInstaller = new ServiceProcessInstaller();

            _serviceInstaller.StartType = ServiceStartMode.Automatic;
            _serviceInstaller.ServiceName = "TS360 ILS Queue Service";
            _serviceInstaller.Description = "The purpose of the ILS Queue windows service application" +
                                            " is to send and process acquisitions data between TS360 and ILS system";

            _serviceProcessInstaller.Account = ServiceAccount.User;

            Installers.Add(_serviceInstaller);
            Installers.Add(_serviceProcessInstaller);
        }
    }
}
