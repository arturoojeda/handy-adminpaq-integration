using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HandySync2
{
    public class HandyMainService : ServiceBase
    {
        public ServiceHost serviceHost = null;
        public HandyMainService()
        {
            ServiceName = "HandySyncService";
        }
        public static void Main()
        {
            ServiceBase.Run(new HandyMainService());
        }
        
        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            DateTime date = DateTime.Parse("2016-02-25", System.Globalization.CultureInfo.CreateSpecificCulture("es-MX"));
            string since = "?since=" + date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            string handyURL = "https://www.handy-app.net/";
            string url = handyURL + "api/salesOrder/list" + since;
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(handyURL);
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            listener.Stop();
        }
    }
}
