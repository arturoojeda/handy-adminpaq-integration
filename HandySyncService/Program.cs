using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HandySyncService
{
    class Program
    {
        public class constantes // Declaración de constantes
        {
            public const int kLongFecha = 24;
            public const int kLongSerie = 12;
            public const int kLongCodigo = 31;
            public const int kLongNombre = 61;
            public const int kLongReferencia = 21;
            public const int kLongDescripcion = 61;
            public const int kLongCuenta = 101;
            public const int kLongMensaje = 3001;
            public const int kLongNombreProducto = 256;
            public const int kLongAbreviatura = 4;
            public const int kLongCodValorClasif = 4;
            public const int kLongDenComercial = 51;
            public const int kLongRepLegal = 51;
            public const int kLongTextoExtra = 51;
            public const int kLongRFC = 21;
            public const int kLongCURP = 21;
            public const int kLongDesCorta = 21;
            public const int kLongNumeroExtInt = 7;
            public const int kLongNumeroExpandido = 31;
            public const int kLongCodigoPostal = 7;
            public const int kLongTelefono = 16;
            public const int kLongEmailWeb = 51;

            public const int kLongSelloSat = 176;
            public const int kLonSerieCertSAT = 21;
            public const int kLongFechaHora = 36;
            public const int kLongSelloCFDI = 176;
            public const int kLongCadOrigComplSAT = 501;
            public const int kLongitudUUID = 37;
            public const int kLongitudRegimen = 101;
            public const int kLongitudMoneda = 61;
            public const int kLongitudFolio = 17;
            public const int kLongitudMonto = 31;
            public const int kLogitudLugarExpedicion = 401;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tMovimiento
        {
            public int aConsecutivo;
            public Double aUnidades;
            public Double aPrecio;
            public Double aCosto;
            public String aCodProdSer;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String aCodAlmacen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongReferencia)]
            public String aReferencia;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String aCodClasificacion;
        }

        [DllImport("MGW_SDK.dll")]
        public static extern Int32 fAltaMovimiento(long aIdDocumento, ref Int32 aIdMovimiento, ref tMovimiento atMovimiento);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static Logger log = new Logger();
        public static void Main()
        {
            log.Info("This is the main Method.");
            log.Info("Before initializing Services To Run");
            // Initialize the service to start
            try
            {
                BaseProcess.MainProcess2();
            }
            catch (Exception ex)
            {
                log.Error("Error when running MainProcess: " + ex.Message);
            }

            //try
            //{
            //    ServiceBase[] ServicesToRun;
            //    ServicesToRun = new ServiceBase[]
            //    {
            //        new HandyWindowsService()
            //    };
            //    log.Info("Before running services.");
            //    // In interactive and debug mode ?
            //    if (Environment.UserInteractive && System.Diagnostics.Debugger.IsAttached)
            //    {
            //        log.Info("Running service in Interactive Mode.");
            //        // Simulate the services execution
            //        RunInteractiveServices(ServicesToRun);
            //    }
            //    else
            //    {
            //        log.Info("Running service in normal mode.");
            //        // Normal service execution
            //        ServiceBase.Run(ServicesToRun);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.Message);
            //}
            
        }

        /// <summary>
        /// Run services in interactive mode
        /// </summary>
        public static void RunInteractiveServices(ServiceBase[] servicesToRun)
        {
            Console.WriteLine();
            Console.WriteLine("Start the services in interactive mode.");
            Console.WriteLine();
            // Get the method to invoke on each service to start it
            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            //Thread mainThread = new Thread(new ThreadStart(BaseProcess.MainProcess));
            //mainThread.Start();
            // Start services loop
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Starting {0} ... ", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.WriteLine("Started");
                
            }

            // Waiting the end
            Console.WriteLine();
            Console.WriteLine("Press a key to stop services et finish process...");
            Console.ReadKey();
            Console.WriteLine();

            // Get the method to invoke on each service to stop it
            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);

            // Stop loop
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Stopping {0} ... ", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("Stopped");
            }
            
            Console.WriteLine();
            Console.WriteLine("All services are stopped.");

            // Waiting a key press to not return to VS directly
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.Write("=== Press a key to quit ===");
                Console.ReadKey();
            }
        }
    }
}
