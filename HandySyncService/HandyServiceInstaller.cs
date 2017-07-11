using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService
{
    [RunInstaller(true)]
    public class HandyServiceInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public HandyServiceInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "HandySyncService";
            Installers.Add(process);
            Installers.Add(service);
            service.StartType = ServiceStartMode.Automatic;
        }
    }
}
