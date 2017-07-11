using HandySyncService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HandySyncService
{
    class HandyWindowsService : ServiceBase
    {
        public ServiceHost adminPaqServiceHost = null;
        public ServiceHost handyServiceHost = null;
        Task process = null;
        static Logger log = new Logger();
        
        public HandyWindowsService()
        {
            log.Info("HandyWindowsService Constructor");
            // Name the Windows Service
            ServiceName = "HandySyncService";
        }

        public static void Main()
        {
            //if (Environment.UserInteractive)
            //{
            //    BaseProcess.MainProcess();
            //}
            //else
            log.Info("Main method in service.");
            try 
            {
                ServiceBase.Run(new HandyWindowsService());
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        protected override void OnStart(string[] args)
        {
            try
            {
                log.Info("Service started.");
                if (adminPaqServiceHost != null)
                {
                    adminPaqServiceHost.Close();
                }
                if (handyServiceHost != null)
                {
                    handyServiceHost.Close();
                }

                // Create a ServiceHost for the AdminPaqService type and 
                // provide the base address.
                adminPaqServiceHost = new ServiceHost(typeof(AdminPaqService));
                // Create a ServiceHost for the HandyService type and 
                // provide the base address.
                handyServiceHost = new ServiceHost(typeof(HandyService));
                // Open the ServiceHostBase to create listeners and start 
                // listening for messages.
                adminPaqServiceHost.Open();
                handyServiceHost.Open();

                Timer t = new Timer();                
                //Start Process
                process = new Task(() => BaseProcess.MainProcess());
                process.Start();                
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }

        protected override void OnStop()
        {
            log.Info("Stopping Service.");
            if (adminPaqServiceHost != null)
            {
                adminPaqServiceHost.Close();
                adminPaqServiceHost = null;
            }
            if (handyServiceHost != null)
            {
                handyServiceHost.Close();
                handyServiceHost = null;
            }
            if (process != null)
            {
                process.Dispose();
                process = null;
            }
        }

        private void InitializeComponent()
        {
            log.Info("HandyWindowsService InitComponent");
            // 
            // HandyWindowsService
            // 
            this.ServiceName = "HandySyncService";

        }
    }
}
